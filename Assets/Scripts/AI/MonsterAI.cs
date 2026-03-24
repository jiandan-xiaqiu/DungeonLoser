using UnityEngine;
using MyGame.AI.BehaviorTree;

namespace MyGame.AI
{
    /// <summary>
    /// 怪物AI组件
    /// </summary>
    public class MonsterAI : MonoBehaviour
    {
        [Header("AI Settings")]
        [Tooltip("检测范围")]
        public float detectionRange = 10f;

        [Tooltip("攻击范围")]
        public float attackRange = 2f;

        [Tooltip("移动速度")]
        public float moveSpeed = 3f;

        [Tooltip("攻击冷却时间")]
        public float attackCooldown = 1f;

        [Header("Animation Settings")]
        [Tooltip("动画参数名称")]
        public string isMovingParam = "IsMoving";
        public string attackTriggerParam = "Attack";
        public string hitTriggerParam = "Hit";
        public string deathTriggerParam = "Death";

        [Tooltip("动画速度")]
        public float animationSpeed = 1f;

        private BehaviorTreeManager _behaviorTreeManager;
        private AIContext _context;
        private Animator _animator;
        private AIState _lastState = AIState.Idle;

        private void Awake()
        {
            // 获取Animator组件
            _animator = GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError("Animator component not found on " + gameObject.name);
            }
            else
            {
                _animator.speed = animationSpeed;
            }

            // 初始化AI上下文
            _context = new AIContext
            {
                aiObject = gameObject,
                detectionRange = detectionRange,
                attackRange = attackRange,
                moveSpeed = moveSpeed,
                attackCooldown = attackCooldown,
                lastAttackTime = 0f,
                currentState = AIState.Idle,
                previousState = AIState.Idle
            };

            // 创建行为树
            BTNode behaviorTree = CreateBehaviorTree();

            // 初始化行为树管理器
            _behaviorTreeManager = gameObject.AddComponent<BehaviorTreeManager>();
            _behaviorTreeManager.Initialize(behaviorTree, _context);
        }

        private void Update()
        {
            // 更新行为树
            _behaviorTreeManager.UpdateBehaviorTree();
            
            // 检查状态变化并更新动画
            UpdateAnimation();
        }

        /// <summary>
        /// 更新动画
        /// </summary>
        private void UpdateAnimation()
        {
            if (_animator == null) return;

            // 检查状态是否发生变化
            if (_context.currentState != _lastState)
            {
                _context.previousState = _lastState;
                _lastState = _context.currentState;
                HandleStateChange(_context.currentState);
            }
        }

        /// <summary>
        /// 处理状态变化
        /// </summary>
        /// <param name="newState">新状态</param>
        private void HandleStateChange(AIState newState)
        {
            switch (newState)
            {
                case AIState.Idle:
                    _animator.SetBool(isMovingParam, false);
                    break;
                    
                case AIState.Moving:
                    _animator.SetBool(isMovingParam, true);
                    break;
                    
                case AIState.Attacking:
                    _animator.SetBool(isMovingParam, false);
                    _animator.SetTrigger(attackTriggerParam);
                    break;
                    
                case AIState.Hit:
                    _animator.SetBool(isMovingParam, false);
                    _animator.SetTrigger(hitTriggerParam);
                    break;
                    
                case AIState.Dead:
                    _animator.SetBool(isMovingParam, false);
                    _animator.SetTrigger(deathTriggerParam);
                    break;
            }
        }

        /// <summary>
        /// 创建行为树
        /// </summary>
        /// <returns>行为树根节点</returns>
        private BTNode CreateBehaviorTree()
        {
            // 创建选择节点作为根节点
            SelectorNode root = new SelectorNode();

            // 创建攻击序列
            SequenceNode attackSequence = new SequenceNode();
            attackSequence.AddChild(new ConditionNode(context => context.target != null));
            attackSequence.AddChild(new ConditionNode(context => 
                Vector3.Distance(context.aiObject.transform.position, context.target.position) <= context.attackRange
            ));
            attackSequence.AddChild(new AttackTargetNode());

            // 创建移动序列
            SequenceNode moveSequence = new SequenceNode();
            moveSequence.AddChild(new FindTargetNode());
            moveSequence.AddChild(new MoveToTargetNode());

            // 添加子节点到根节点
            root.AddChild(attackSequence);
            root.AddChild(moveSequence);
            root.AddChild(new IdleNode());

            return root;
        }

        /// <summary>
        /// 处理受击
        /// </summary>
        public void OnHit()
        {
            _context.currentState = AIState.Hit;
        }

        /// <summary>
        /// 处理死亡
        /// </summary>
        public void OnDeath()
        {
            _context.currentState = AIState.Dead;
            
            // 立即处理死亡状态变化，确保死亡动画被触发
            if (_context.currentState != _lastState)
            {
                _lastState = _context.currentState;
                HandleStateChange(_context.currentState);
            }
            
            enabled = false; // 禁用AI更新
        }
    }
}
