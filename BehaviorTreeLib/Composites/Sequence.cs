using System.Collections;
using System.Collections.Generic;

namespace BehaviorTreeLib.Composite
{
    public class Sequence : BehaviorTreeNode
    {
        private int _currentPosition;

        /// <summary>
        /// 
        /// </summary>
        public Sequence()
        {
            _children = new List<BehaviorTreeNode>();
            _currentPosition = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="children"></param>
        public Sequence(List<BehaviorTreeNode> children)
        {
            _children = children;
            _currentPosition = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            foreach (BehaviorTreeNode node in _children)
                node.Init();
            _currentPosition = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override BehaviorReturnCode Run()
        {
            if (_children.Count == 0) return BehaviorReturnCode.BT_SUCCESS;

            if (_currentPosition == -1) Init();

            BehaviorTreeNode currentRunningNode = _children[_currentPosition];
            BehaviorReturnCode status = BehaviorReturnCode.BT_FAILURE;
            while ((status = currentRunningNode.Run()) == BehaviorReturnCode.BT_SUCCESS)
            {
                _currentPosition++;
                // Fallan todos los hijos
                if (_currentPosition == _children.Count)
                {
                    _currentPosition = -1;
                    return status;
                }
                currentRunningNode = _children[_currentPosition];
            }

            if (status == BehaviorReturnCode.BT_FAILURE)
                _currentPosition = -1;

            return status;
        }
    }
}
