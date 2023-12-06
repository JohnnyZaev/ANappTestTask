using System;
using System.Collections;

namespace StateMachine.States
{
    /// <summary>
    /// A generic empty state. Pass onExecute action into Constructor to run once when entering the state
    /// (or null to do nothing)
    /// </summary>
    public class State : AbstractState
    {
        private readonly Action _mOnExecute;

        /// <param name="onExecute">An event that is invoked when the state is executed</param>
        ///
        // Constructor takes delegate to execute and optional name (for debugging)
        public State(Action onExecute, string stateName = nameof(State))
        {
            _mOnExecute = onExecute;
            Name = stateName;
        }

        public override IEnumerator Execute()
        {
            yield return null;

            if (MDebug)
                base.DebugState();

            // Invokes the m_OnExecute Action if it exists
            _mOnExecute?.Invoke();
        }
    }
}