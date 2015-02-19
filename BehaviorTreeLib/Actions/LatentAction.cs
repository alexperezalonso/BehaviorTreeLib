using System;
using System.Collections.Generic;

namespace BehaviorTreeLib.Actions
{
    public enum ActionStatus
    {
        /// Indica que la acción latente aún no se ha empezado
        /// a ejecutar
        READY,
        /// Indica que la acción latente está ejecutándose,
        /// y por lo tanto necesita ciclos de CPU
        RUNNING,
        /// Indica que la acción latente está suspendida a
        /// la espera de algún evento externo, y por tanto no
        /// necesita (al menos por el momento) ciclos de CPU.
        SUSPENDED,
        /// Indica que la acción latente ha terminado su
        /// tarea con éxito.
        SUCCESS,
        /// Indica que la acción latente ha terminado su
        /// tarea con fallo.
        FAIL
    }

    /// <summary>
    /// 
    /// </summary>
    class LatentAction
    {

        public ActionStatus _status;
        /// <summary>
        /// Delegate functions
        /// </summary>
        private BehaviorActionDelegate _OnStart, _OnRun, _OnAbort, _OnStop;
        private bool _stopping;

        #region Properties BehaviorActionDelegate setters
        public BehaviorActionDelegate OnStart
        {
            set { _OnStart = value; }
        }
        public BehaviorActionDelegate OnRun
        {
            set { _OnRun = value; }
        }
        public BehaviorActionDelegate OnAbort
        {
            set { _OnAbort = value; }
        }
        public BehaviorActionDelegate OnStop
        {
            set { _OnStop = value; }
        }
        #endregion

        /// <summary>
        /// Return the current status (READY, RUNNING, SUSPEND, SUCCESS, FAIL)
        /// </summary>
        public ActionStatus Status
        {
            get { return _status; }
        }

        /// <summary>
        /// 
        /// </summary>
        public LatentAction(BehaviorActionDelegate OnStart, BehaviorActionDelegate OnRun,
            BehaviorActionDelegate OnAbort, BehaviorActionDelegate OnStop)
        {
            _OnStart = OnStart; _OnRun = OnRun; _OnAbort = OnAbort; _OnStop = OnStop;

            _status = ActionStatus.READY;
            _stopping = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public LatentAction(BaseAction action)
        {
            _OnStart = action.OnStart; _OnRun = action.OnRun;
            _OnAbort = action.OnAbort; _OnStop = action.OnStop;

            _status = ActionStatus.READY;
            _stopping = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionStatus Update()
        {
            // ¿Hay que empezar la tarea?
            if (_status == ActionStatus.READY)
            {
                _status = _OnStart();
            }

            // Llamamos al Tick, si el OnStart no terminó
            // con la ejecución
            if ((_status == ActionStatus.RUNNING))
            {
                _status = _OnRun();
                // Si hemos pasado de RUNNING a un estado de finalización (SUCCESS o FAIL) 
                // aún tenemos que parar
                if (_status == ActionStatus.SUCCESS || _status == ActionStatus.FAIL)
                    _stopping = true;
            }

            // Si OnRun() terminó, llamamos al OnStop() y terminamos;
            // si estábamos terminando (se solicitó la terminación
            // de forma asíncrona), también.
            if ((_status == ActionStatus.SUCCESS || _status == ActionStatus.FAIL) && (_stopping))
            {
                // Paramos y decimos que ya no hay que volver a ejecutar OnStop
                _OnStop();
                ActionStatus auxStatus = _status;
                _status = ActionStatus.READY;
                _stopping = false;
                return auxStatus;
            }
            return _status;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            if (_status != ActionStatus.READY)
            {
                // Si estamos en ejecución (normal o suspendida) 
                // tenemos que llamar a onAbort (porque en realidad
                // abortamos la acción)
                if (_status == ActionStatus.RUNNING || _status == ActionStatus.SUSPENDED)
                    _OnAbort();
                // Si hemos terminado la ejecución pero no hemos parado (OnStop) tenemos que 
                // llamar a OnStop
                if ((_status == ActionStatus.SUCCESS || _status == ActionStatus.FAIL) && (_stopping))
                    _OnStop();
                // Dejamos el estado listo para volver a ejecutarla
                _status = ActionStatus.READY;
                _stopping = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Abort()
        {
            _stopping = false;
            _status = _OnAbort();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        public void Finish(bool success)
        {
            _status = (success ? ActionStatus.SUCCESS : ActionStatus.FAIL);
            _stopping = true;
        }

        public delegate ActionStatus BehaviorActionDelegate();
    }
}
