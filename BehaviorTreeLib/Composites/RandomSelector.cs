using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeLib.Composite
{
    public class RandomSelector : BehaviorTreeNode
    {
        private int _currentPosition;

        public RandomSelector()
        {
            _children = new List<BehaviorTreeNode>();
            _currentPosition = -1;
        }


        public RandomSelector(List<BehaviorTreeNode> children)
        {
            _children = children;
            _currentPosition = -1;
        }

        // <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            foreach (BehaviorTreeNode node in _children)
                node.Init();

            _currentPosition = 0;
        }

        // <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override BehaviorReturnCode Run()
        {
            if (_children.Count == 0) return BehaviorReturnCode.BT_SUCCESS;

            if (_currentPosition == -1)
                Init();

            System.Random random = new System.Random(DateTime.Now.Millisecond);
            int randomNumber = 0;
            BehaviorReturnCode status = BehaviorReturnCode.BT_FAILURE;

            if (_currentPosition == -1)
                randomNumber = random.Next(0, _children.Count);
            else
                randomNumber = _currentPosition;

            BehaviorTreeNode currentRunningNode = _children[randomNumber];
            status = currentRunningNode.Run();
            if (status == BehaviorReturnCode.BT_SUCCESS || status == BehaviorReturnCode.BT_FAILURE)
                _currentPosition = -1;
                
            return status;
        }
    }
}
