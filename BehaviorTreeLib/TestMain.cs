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
            BehaviorTreeNode bt;
            LatentAction actionA = new LatentAction(
                delegate()
                {
                    return ActionStatus.RUNNING;
                }, 
                delegate()
                {
                    return ActionStatus.SUCCESS;
                }, 
                delegate()
                {
                    return ActionStatus.READY;
                },
                delegate()
                {
                    return ActionStatus.READY;
                });

            LatentAction actionB = new LatentAction(
                delegate()
                {
                    return ActionStatus.RUNNING;
                },
                delegate()
                {
                    return ActionStatus.FAIL;
                },
                delegate()
                {
                    return ActionStatus.READY;
                },
                delegate()
                {
                    return ActionStatus.READY;
                });

            bt = new Selector(
                new Sequence( 
                    new ActionNode(actionA),
                    new ActionNode(actionB)),
                new Selector(
                    new ActionNode(actionA),
                    new ActionNode(actionB)
                    )
                );

            bt.Run();
        }
    }
}
