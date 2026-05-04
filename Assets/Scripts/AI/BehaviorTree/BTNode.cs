using UnityEngine;

namespace MyGame.AI.BehaviorTree
{
    /// <summary>
    /// 行为树节点状态
    /// </summary>
    public enum NodeStatus
    {
        Success,  // 成功
        Failure,  // 失败
        Running   // 运行中
    }

    /// <summary>
    /// 行为树节点基类
    /// </summary>
    public abstract class BTNode
    {
        /// <summary>
        /// 执行节点逻辑
        /// </summary>
        /// <param name="context">AI上下文</param>
        /// <returns>节点状态</returns>
        public abstract NodeStatus Execute(AIContext context);

        /// <summary>
        /// 重置节点状态
        /// </summary>
        public virtual void Reset() { }
    }

    /// <summary>
    /// AI上下文，包含AI执行所需的信息
    /// </summary>
    public class AIContext
    {
        public GameObject aiObject;          // AI对象
        public Transform target;             // 目标（通常是玩家）
        public float detectionRange;         // 检测范围
        public float attackRange;           // 攻击范围
        public float moveSpeed;             // 移动速度
        public float attackCooldown;         // 攻击冷却时间
        public float attackDamage;           // 攻击伤害
        public float lastAttackTime;         // 上次攻击时间
        public AIState currentState;         // 当前AI状态
        public AIState previousState;       // 上一个AI状态
    }

    /// <summary>
    /// AI状态
    /// </summary>
    public enum AIState
    {
        Idle,       // 待机
        Moving,     // 移动
        Attacking,  // 攻击
        Hit,        // 受击
        Dead        // 死亡
    }
}
