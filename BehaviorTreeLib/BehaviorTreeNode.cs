using System.Collections;
using System.Collections.Generic;

namespace BehaviorTreeLib
{
    public enum BehaviorReturnCode
    {
        BT_FAILURE,
        BT_SUCCESS,
        BT_RUNNING
    }

    public abstract class BehaviorTreeNode
    {
        // Lista de hijos de está tarea
        protected List<BehaviorTreeNode> _children;
        protected BehaviorReturnCode ReturnCode;

        public BehaviorTreeNode() { }
        public void Add(BehaviorTreeNode child)
        {
            _children.Add(child);
        }
        public void AddRange(params BehaviorTreeNode[] children)
        {
            _children.AddRange(children);
        }

        public abstract void Init();
        public abstract BehaviorReturnCode Run();
    }
}