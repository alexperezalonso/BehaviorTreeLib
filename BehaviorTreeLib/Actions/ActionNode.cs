using System;
using System.Collections.Generic;

namespace BehaviorTreeLib.Actions
{


    class ActionNode : BehaviorTreeNode
    {

        LatentAction _action;

        public ActionNode(LatentAction action)
        {
            _action = action;

        }
        public override void Init()
        {
            if (_action.Status != ActionStatus.READY)
                _action.Reset();
        }

        public override BehaviorReturnCode Run()
        {
            ActionStatus status = _action.Status;

            if (status == ActionStatus.SUCCESS)
                return BehaviorReturnCode.BT_SUCCESS;
            if (status == ActionStatus.FAIL)
                return BehaviorReturnCode.BT_FAILURE;

            // Si no ha terminado la LA la ejecutamos
            status = _action.Update();

            // Transformamos el resultado de ejecutar la LA en un resultado de estado del árbol (BEHAVIOR_STATUS)
            if (status == ActionStatus.SUCCESS)
                return BehaviorReturnCode.BT_SUCCESS;
            if (status == ActionStatus.FAIL)
                return BehaviorReturnCode.BT_FAILURE;


            return BehaviorReturnCode.BT_RUNNING;
        }

        
    }
}
