using UnityEngine;

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

            Vector3 direction = (context.target.position - context.aiObject.transform.position).normalized;
            float distance = Vector3.Distance(context.aiObject.transform.position, context.target.position);

            if (distance > context.attackRange)
            {
                context.aiObject.transform.position += direction * context.moveSpeed * Time.deltaTime;
                context.currentState = AIState.Moving;
                return NodeStatus.Running;
            }
            else
            {
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

            // 执行攻击逻辑
            context.lastAttackTime = Time.time;
            context.currentState = AIState.Attacking;
            
            // 这里可以添加具体的攻击代码，比如播放攻击动画、造成伤害等
            
            return NodeStatus.Success;
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
