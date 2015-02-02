using System;
using System.Collections.Generic;

namespace BehaviorTreeLib.Conditions
{
    public class ConditionNode : BehaviorTreeNode
    {
        private ConditionDelegate _Condition;

        public ConditionNode(ConditionDelegate dlg)
        {
            _Condition = dlg;
        }

        public override void Init() { }

        public override BehaviorReturnCode Run()
        {
            if (_Condition()) return BehaviorReturnCode.BT_SUCCESS;
            return BehaviorReturnCode.BT_FAILURE;
        }

        public delegate bool ConditionDelegate();
    }
}
