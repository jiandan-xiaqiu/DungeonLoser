using UnityEngine;
using MyGame;

namespace MyGame.AI.BehaviorTree
{
    /// <summary>
    /// 装饰节点基类
    /// </summary>
    public abstract class DecoratorNode : BTNode
    {
        protected BTNode _child;

        public void SetChild(BTNode child)
        {
            _child = child;
        }
    }

    /// <summary>
    /// 重复节点
    /// 重复执行子节点，直到达到指定次数或子节点返回失败
    /// </summary>
    public class RepeatNode : DecoratorNode
    {
        private int _times;
        private int _currentCount;

        public RepeatNode(int times)
        {
            _times = times;
        }

        public override NodeStatus Execute(AIContext context)
        {
            if (_currentCount < _times)
            {
                NodeStatus status = _child.Execute(context);
                if (status == NodeStatus.Success)
                {
                    _currentCount++;
                    _child.Reset();
                    return NodeStatus.Running;
                }
                if (status == NodeStatus.Failure)
                {
                    _currentCount = 0;
                    return NodeStatus.Failure;
                }
                return NodeStatus.Running;
            }
            
            _currentCount = 0;
            return NodeStatus.Success;
        }

        public override void Reset()
        {
            _currentCount = 0;
            _child.Reset();
        }
    }

    /// <summary>
    /// 条件节点
    /// 用于判断条件是否满足
    /// </summary>
    public class ConditionNode : BTNode
    {
        private System.Func<AIContext, bool> _condition;

        public ConditionNode(System.Func<AIContext, bool> condition)
        {
            _condition = condition;
        }

        public override NodeStatus Execute(AIContext context)
        {
            return _condition(context) ? NodeStatus.Success : NodeStatus.Failure;
        }
    }

    /// <summary>
    /// 动作节点基类
    /// </summary>
    public abstract class ActionNode : BTNode
    {}

    /// <summary>
    /// 移动到目标节点
    /// </summary>
    public class MoveToTargetNode : ActionNode
    {
        public override NodeStatus Execute(AIContext context)
        {
            if (context.target == null)
            {
                return NodeStatus.Failure;
            }

            // 只考虑X和Y轴
            Vector2 direction = new Vector2(
                context.target.position.x - context.aiObject.transform.position.x,
                context.target.position.y - context.aiObject.transform.position.y
            ).normalized;
            
            float distance = Vector2.Distance(
                new Vector2(context.aiObject.transform.position.x, context.aiObject.transform.position.y),
                new Vector2(context.target.position.x, context.target.position.y)
            );

            if (distance > context.attackRange)
            {
                // 只修改X和Y轴，锁定Z轴
                Vector3 newPosition = context.aiObject.transform.position;
                newPosition.x += direction.x * context.moveSpeed * Time.deltaTime;
                newPosition.y += direction.y * context.moveSpeed * Time.deltaTime;
                newPosition.z = 0f; // 锁定Z轴
                context.aiObject.transform.position = newPosition;
                
                context.currentState = AIState.Moving;
                return NodeStatus.Running;
            }
            else
            {
                // 确保Z轴为0
                Vector3 position = context.aiObject.transform.position;
                position.z = 0f;
                context.aiObject.transform.position = position;
                
                context.currentState = AIState.Idle;
                return NodeStatus.Success;
            }
        }
    }

    /// <summary>
    /// 攻击目标节点
    /// </summary>
    public class AttackTargetNode : ActionNode
    {
        public override NodeStatus Execute(AIContext context)
        {
            if (context.target == null)
            {
                return NodeStatus.Failure;
            }

            float distance = Vector3.Distance(context.aiObject.transform.position, context.target.position);
            if (distance > context.attackRange)
            {
                return NodeStatus.Failure;
            }

            if (Time.time - context.lastAttackTime < context.attackCooldown)
            {
                return NodeStatus.Running;
            }

            // 执行攻击逻辑 - 伤害结算
            context.lastAttackTime = Time.time;
            context.currentState = AIState.Attacking;
            
            // 对目标造成伤害
            DealDamageToTarget(context);
            
            return NodeStatus.Success;
        }
        
        /// <summary>
        /// 对目标造成伤害
        /// </summary>
        private void DealDamageToTarget(AIContext context)
        {
            if (context.target == null || context.attackDamage <= 0f)
                return;
            
            // 从目标身上获取 CharacterManager 并造成伤害
            CharacterManager targetStats = context.target.GetComponent<CharacterManager>();
            if (targetStats != null)
            {
                targetStats.TakeDamage(context.attackDamage);
            }
        }
    }

    /// <summary>
    /// 寻找目标节点
    /// </summary>
    public class FindTargetNode : ActionNode
    {
        public override NodeStatus Execute(AIContext context)
        {
            // 查找距离最近的玩家
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length == 0)
            {
                context.target = null;
                return NodeStatus.Failure;
            }

            GameObject closestPlayer = null;
            float closestDistance = Mathf.Infinity;

            foreach (var player in players)
            {
                float distance = Vector3.Distance(context.aiObject.transform.position, player.transform.position);
                if (distance < closestDistance && distance <= context.detectionRange)
                {
                    closestDistance = distance;
                    closestPlayer = player;
                }
            }

            if (closestPlayer != null)
            {
                context.target = closestPlayer.transform;
                return NodeStatus.Success;
            }
            else
            {
                context.target = null;
                return NodeStatus.Failure;
            }
        }
    }

    /// <summary>
    /// 待机节点
    /// </summary>
    public class IdleNode : ActionNode
    {
        public override NodeStatus Execute(AIContext context)
        {
            context.currentState = AIState.Idle;
            return NodeStatus.Success;
        }
    }
}
