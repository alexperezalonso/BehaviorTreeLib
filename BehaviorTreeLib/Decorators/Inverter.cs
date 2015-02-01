using System;
using System.Collections.Generic;

namespace BehaviorTreeLib.Decorators
{
    public class Inverter : Decorator
    {
        public Inverter()
        {
            _children = new List<BehaviorTreeNode>(1);
        }

        public Inverter(BehaviorTreeNode child)
        {
            _children = new List<BehaviorTreeNode>(1);
            _children.Add(child);
        }

        public override void Init()
        {
            _children[0].Init();
        }

        public override BehaviorReturnCode Run()
        {
            BehaviorReturnCode status = _children[0].Run();
            if (status == BehaviorReturnCode.BT_FAILURE)
                return BehaviorReturnCode.BT_SUCCESS;
            else if (status == BehaviorReturnCode.BT_SUCCESS)
                return BehaviorReturnCode.BT_FAILURE;

            return status;
        }
    }
}
