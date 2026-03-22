using UnityEngine;

namespace MyGame.AI.BehaviorTree
{
    /// <summary>
    /// 行为树管理器
    /// </summary>
    public class BehaviorTreeManager : MonoBehaviour
    {
        private BTNode _rootNode;
        private AIContext _context;

        /// <summary>
        /// 初始化行为树
        /// </summary>
        /// <param name="rootNode">根节点</param>
        /// <param name="context">AI上下文</param>
        public void Initialize(BTNode rootNode, AIContext context)
        {
            _rootNode = rootNode;
            _context = context;
        }

        /// <summary>
        /// 更新行为树
        /// </summary>
        public void UpdateBehaviorTree()
        {
            if (_rootNode != null && _context != null && _context.currentState != AIState.Dead)
            {
                _rootNode.Execute(_context);
            }
        }

        /// <summary>
        /// 重置行为树
        /// </summary>
        public void ResetBehaviorTree()
        {
            if (_rootNode != null)
            {
                _rootNode.Reset();
            }
        }
    }
}
