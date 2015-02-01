using System;
using System.Collections.Generic;

namespace BehaviorTreeLib.Composite
{
    public class NonDeterministicSequence : Sequence
    {
        private int _currentPosition;

        /// <summary>
        /// 
        /// </summary>
        public NonDeterministicSequence()
        {
            _children = new List<BehaviorTreeNode>();
            _currentPosition = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="children"></param>
        public NonDeterministicSequence(List<BehaviorTreeNode> children)
        {
            _children = children;
            _currentPosition = -1;
        }

        public override void Init()
        {
            foreach (BehaviorTreeNode node in _children)
                node.Init();

            // Shuffle children list
            Shuffle();

            _currentPosition = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override BehaviorReturnCode Run()
        {
            return base.Run();
        }

        /// <summary>
        /// Reorder the children pseudo-randomly
        /// </summary>
        private void Shuffle()
        {
            int n = _children.Count;
            System.Random rnd = new System.Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                BehaviorTreeNode value = _children[k];
                _children[k] = _children[n];
                _children[n] = value;
            }
        }
    }
}
