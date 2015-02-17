using System;
using System.Collections.Generic;

namespace BehaviorTreeLib.Composites
{
    public enum FailurePolicy
    {
        FAIL_ON_ONE,
        FAIL_ON_ALL
    }

    public enum SuccessPolicy
    {
        SUCCEED_ON_ONE,
        SUCCEED_ON_ALL
    }

    public class Parallel : BehaviorTreeNode
    {

        private FailurePolicy _failurePolicy = FailurePolicy.FAIL_ON_ONE;
        private SuccessPolicy _successPolicy = SuccessPolicy.SUCCEED_ON_ALL;
        private List<BehaviorReturnCode> _childrenStatus = null;

        /// <summary>
        /// 
        /// </summary>
        public Parallel()
        {
            _children = new List<BehaviorTreeNode>();
            _childrenStatus = new List<BehaviorReturnCode>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="failurePolicy"></param>
        /// <param name="successPolicy"></param>
        /// <param name="children"></param>
        public Parallel(FailurePolicy failurePolicy, SuccessPolicy successPolicy,
            List<BehaviorTreeNode> children)
        {
            _children = children;
            _childrenStatus = new List<BehaviorReturnCode>();
            _failurePolicy = failurePolicy;
            _successPolicy = successPolicy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="failurePolicy"></param>
        /// <param name="successPolicy"></param>
        /// <param name="children"></param>
        public Parallel(FailurePolicy failurePolicy, SuccessPolicy successPolicy,
            params BehaviorTreeNode[] children)
        {
            _children = new List<BehaviorTreeNode>();
            _children.AddRange(children);
            _childrenStatus = new List<BehaviorReturnCode>();
            _failurePolicy = failurePolicy;
            _successPolicy = successPolicy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        private void setChildrenStatus(BehaviorReturnCode code)
        {
            for (int i = 0; i < _childrenStatus.Count; i++)
                _childrenStatus[i] = code;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            foreach (BehaviorTreeNode node in _children)
                node.Init();
            if (_children.Count > _childrenStatus.Count)
            {
                for (int i = _childrenStatus.Count; i < _children.Count; i++)
                {
                    _childrenStatus.Add(BehaviorReturnCode.BT_RUNNING);
                }
            }
            setChildrenStatus(BehaviorReturnCode.BT_RUNNING);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override BehaviorReturnCode Run()
        {
            BehaviorReturnCode status = BehaviorReturnCode.BT_FAILURE;
            for (int i = 0; i < _children.Count; i++)
            {
                status = _children[i].Run();
                if (status == BehaviorReturnCode.BT_FAILURE &&
                    _failurePolicy == FailurePolicy.FAIL_ON_ONE)
                {
                    Init();
                    return BehaviorReturnCode.BT_FAILURE;
                }
                else
                    _childrenStatus[i] = status;
            }

            // Recorremos los valores de retorno comprobando si se cumple alguna de la condiciones.
            bool sawSuccess = false;
            bool sawAllFails = true;
            bool sawAllSuccess = true;
            for (int i = 0; i < _childrenStatus.Count; i++)
            {
                switch (_childrenStatus[i])
                {
                    case BehaviorReturnCode.BT_SUCCESS:
                        if (_successPolicy == SuccessPolicy.SUCCEED_ON_ONE &&
                            _failurePolicy == FailurePolicy.FAIL_ON_ALL)
                        {
                            Init();
                            return BehaviorReturnCode.BT_SUCCESS;
                        }
                        sawSuccess = true;
                        sawAllFails = false;
                        break;
                    case BehaviorReturnCode.BT_FAILURE:
                        if (_failurePolicy == FailurePolicy.FAIL_ON_ONE)
                        {
                            Init();
                            return BehaviorReturnCode.BT_FAILURE;
                        }
                        sawAllSuccess = false;
                        break;
                    case BehaviorReturnCode.BT_RUNNING:
                        sawAllFails = false;
                        sawAllSuccess = false;
                        if (_failurePolicy == FailurePolicy.FAIL_ON_ALL &&
                            _successPolicy == SuccessPolicy.SUCCEED_ON_ALL)
                        {
                            return BehaviorReturnCode.BT_RUNNING;
                        }
                        break;
                }
            }

            if (_failurePolicy == FailurePolicy.FAIL_ON_ALL && sawAllFails)
            {
                Init();
                return BehaviorReturnCode.BT_FAILURE;
            }
            else if (_successPolicy == SuccessPolicy.SUCCEED_ON_ALL && sawAllSuccess
                || _successPolicy == SuccessPolicy.SUCCEED_ON_ONE && sawSuccess)
            {
                Init();
                return BehaviorReturnCode.BT_SUCCESS;
            }
            else
            {
                return BehaviorReturnCode.BT_RUNNING;
            }
        }
    }
}
