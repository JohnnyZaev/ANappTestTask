using System;
using System.Collections;
using UnityEngine;

namespace StateMachine.States
{
    /// <summary>
    /// Delays the state-machine for the set amount. Pass in an Action<float> to run
    /// every frame while waiting (e.g. for a loading bar on the SplashScreen).
    /// </summary>
    public class DelayState : AbstractState
    {
        private readonly float _mDelayInSeconds;

        // Optional Action to run every frame while waiting
        private readonly Action<float> _mProgressUpdated;

        // Optional Action to run when execution completes
        private readonly Action _mOnExit;

        /// <param name="delayInSeconds">delay in seconds</param>
        public DelayState(float delayInSeconds, Action<float> onUpdate = null, Action onExit = null, string stateName = nameof(DelayState))
        {
            _mDelayInSeconds = delayInSeconds;
            _mProgressUpdated = onUpdate;
            _mOnExit = onExit;
            Name = stateName;
        }

        public override IEnumerator Execute()
        {
            var startTime = Time.time;

            if (MDebug)
                base.DebugState();

            while (Time.time - startTime < _mDelayInSeconds)
            {
                yield return null;
                float progressValue = (Time.time - startTime) / _mDelayInSeconds;
                _mProgressUpdated?.Invoke(progressValue*100);
            }
        }

        public override void Exit()
        {
            _mOnExit?.Invoke();
        }
    }
}
