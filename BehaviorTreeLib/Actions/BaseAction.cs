using System;
using System.Collections.Generic;

namespace BehaviorTreeLib.Actions
{
    public class BaseAction
    {
        public virtual ActionStatus OnStart() { return ActionStatus.RUNNING; }
        public virtual ActionStatus OnRun() { return ActionStatus.RUNNING; }
        public virtual ActionStatus OnAbort() { return ActionStatus.READY; }
        public virtual ActionStatus OnStop() { return ActionStatus.READY; }
    }

}

