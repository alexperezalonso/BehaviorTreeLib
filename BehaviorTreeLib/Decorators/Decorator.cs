using System;
using System.Collections.Generic;

namespace BehaviorTreeLib.Decorators
{
    public abstract class  Decorator : BehaviorTreeNode
    {

        public Decorator() { }
        public abstract override void Init();
        public abstract override BehaviorReturnCode Run();
    }
}
