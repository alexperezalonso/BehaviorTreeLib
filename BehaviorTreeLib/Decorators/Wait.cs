using System;
using System.Collections.Generic;

namespace BehaviorTreeLib.Decorators
{
    class Wait : Decorator
    {
        private float _count;
        private float _waitTime;
        private TimerDelegate _timerDlg;

        public Wait(float duration, TimerDelegate dlg, BehaviorTreeNode child)
        {
            _children = new List<BehaviorTreeNode>(1);
            _children.Add(child);
            _waitTime = duration;
            _count = 0;
            _timerDlg = dlg;
        }

        public override void Init()
        {
            _count = 0;
        }

        public override BehaviorReturnCode Run()
        {
            _count += _timerDlg();

            if (_count >= _waitTime)
            {

                return _children[0].Run();
            }

            return BehaviorReturnCode.BT_RUNNING;
        }

        /// <summary>
        /// Timer delegate. This might be a function that returns Time.DeltaTime (Unity3D)
        /// </summary>
        /// <returns></returns>
        public delegate float TimerDelegate();
    }
}
