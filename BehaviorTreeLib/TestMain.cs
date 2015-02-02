using BehaviorTreeLib.Composite;
using BehaviorTreeLib.Action;
using BehaviorTreeLib.Decorators;
using System;
using System.Collections.Generic;

namespace BehaviorTreeLib
{
    class TestMain
    {
        static void Main(string[] args)
        {
            /*** Latent Actions ***/
            LatentAction actionA = new LatentAction(
                delegate() { return ActionStatus.RUNNING; },
                delegate()
                {
                    System.Diagnostics.Debug.WriteLine("actionA");
                    return ActionStatus.SUCCESS;
                },
                delegate() { return ActionStatus.READY; },
                delegate() { return ActionStatus.READY; });

            LatentAction actionB = new LatentAction(
                delegate() { return ActionStatus.RUNNING; },
                delegate()
                {
                    System.Diagnostics.Debug.WriteLine("actionB");
                    return ActionStatus.FAIL;
                },
                delegate() { return ActionStatus.READY; },
                delegate() { return ActionStatus.READY; });

            LatentAction actionC = new LatentAction(
                delegate() { return ActionStatus.RUNNING; },
                delegate()
                {
                    System.Diagnostics.Debug.WriteLine("actionC");
                    return ActionStatus.SUCCESS;
                },
                delegate() { return ActionStatus.READY; },
                delegate() { return ActionStatus.READY; });

            LatentAction actionD = new LatentAction(
                delegate() { return ActionStatus.RUNNING; },
                delegate()
                {
                    System.Diagnostics.Debug.WriteLine("actionD");
                    return ActionStatus.FAIL;
                },
                delegate() { return ActionStatus.READY; },
                delegate() { return ActionStatus.READY; });

            /*** Behavior Tree ***/
            BehaviorTreeNode bt = new Selector(
                new Sequence(
                    new ActionNode(actionA),
                    new ActionNode(actionB)),
                new Selector(
                    new Limit(5, new ActionNode(actionC)),
                    new ActionNode(actionD)
                    )
                );


            while (bt.Run() != BehaviorReturnCode.BT_FAILURE) { }
            //bt.Run();
        }
    }
}
