using System.Collections.Generic;
using UnityEngine;

namespace MyGame.AI.BehaviorTree
{
    /// <summary>
    /// 序列节点
    /// 按顺序执行子节点，只要有一个子节点失败就返回失败
    /// 所有子节点成功才返回成功
    /// </summary>
    public class SequenceNode : BTNode
    {
        private List<BTNode> _children = new List<BTNode>();
        private int _currentIndex = 0;

        public void AddChild(BTNode child)
        {
            _children.Add(child);
        }

        public override NodeStatus Execute(AIContext context)
        {
            while (_currentIndex < _children.Count)
            {
                NodeStatus status = _children[_currentIndex].Execute(context);
                
                if (status == NodeStatus.Failure)
                {
                    _currentIndex = 0;
                    return NodeStatus.Failure;
                }
                
                if (status == NodeStatus.Running)
                {
                    return NodeStatus.Running;
                }
                
                _currentIndex++;
            }
            
            _currentIndex = 0;
            return NodeStatus.Success;
        }

        public override void Reset()
        {
            _currentIndex = 0;
            foreach (var child in _children)
            {
                child.Reset();
            }
        }
    }

    /// <summary>
    /// 选择节点
    /// 按顺序执行子节点，只要有一个子节点成功就返回成功
    /// 所有子节点失败才返回失败
    /// </summary>
    public class SelectorNode : BTNode
    {
        private List<BTNode> _children = new List<BTNode>();
        private int _currentIndex = 0;

        public void AddChild(BTNode child)
        {
            _children.Add(child);
        }

        public override NodeStatus Execute(AIContext context)
        {
            while (_currentIndex < _children.Count)
            {
                NodeStatus status = _children[_currentIndex].Execute(context);
                
                if (status == NodeStatus.Success)
                {
                    _currentIndex = 0;
                    return NodeStatus.Success;
                }
                
                if (status == NodeStatus.Running)
                {
                    return NodeStatus.Running;
                }
                
                _currentIndex++;
            }
            
            _currentIndex = 0;
            return NodeStatus.Failure;
        }

        public override void Reset()
        {
            _currentIndex = 0;
            foreach (var child in _children)
            {
                child.Reset();
            }
        }
    }
}
