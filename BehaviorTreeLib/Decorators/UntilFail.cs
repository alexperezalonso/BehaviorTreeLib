using System;
using System.Collections.Generic;

namespace BehaviorTreeLib.Decorators
{
    public class UntilFail : Decorator
    {

        public UntilFail()
        {
            _children = new List<BehaviorTreeNode>(1);
        }

        public UntilFail(BehaviorTreeNode child)
        {
            _children = new List<BehaviorTreeNode>(1);
            _children.Add(child);
        }

        public override void Init()
        {
            _children[0].Init();
        }

        /// <summary>
        /// Run the child until it fails.
        /// </summary>
        /// <returns>Success if the child fails or Running if not</returns>
        public override BehaviorReturnCode Run()
        {
            BehaviorReturnCode status = _children[0].Run();
            if (status != BehaviorReturnCode.BT_FAILURE)
            {
                Init();
                return BehaviorReturnCode.BT_RUNNING;
            }

            return BehaviorReturnCode.BT_SUCCESS;
        }
    }
}
