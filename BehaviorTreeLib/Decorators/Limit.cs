using System;
using System.Collections.Generic;

namespace BehaviorTreeLib.Decorators
{
    public class Limit : Decorator
    {
        private int _runLimit;
        private int _count;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runLimit"></param>
        public Limit(int runLimit)
        {
            _children = new List<BehaviorTreeNode>(1);
            _runLimit = runLimit;
            _count = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runLimit"></param>
        /// <param name="child"></param>
        public Limit(int runLimit, BehaviorTreeNode child)
        {
            _children = new List<BehaviorTreeNode>(1);
            _children.Add(child);
            _runLimit = runLimit;
            _count = 0;
        }

        public override void Init()
        {
            _children[0].Init();
        }

        public override BehaviorReturnCode Run()
        {
            if (_count >= _runLimit)
                return BehaviorReturnCode.BT_FAILURE;

            _count++;
            return _children[0].Run();
        }
    }
}
