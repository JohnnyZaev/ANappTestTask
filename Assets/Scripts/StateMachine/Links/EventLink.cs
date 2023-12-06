using System;
using StateMachine.Interfaces;

namespace StateMachine.Links
{
    /// <summary>
    /// A link that listens for a specific event and becomes open for transition if the event is raised.
    /// If the current state is linked to next step by this link type,
    /// The state machine waits for the event to be triggered and then moves to the next step.
    /// </summary>
    public class EventLink : ILink
    {
        private IState _mNextState;

        private Action _mGameEvent;
        private bool _mEventRaised;
        
        // Pass a GameEvent (System.Action) and the next state into the Constructor.
        public EventLink(Action gameEvent, IState nextState)
        {
            _mGameEvent = gameEvent;
            _mNextState = nextState;
        }

        public bool Validate(out IState nextState)
        {
            nextState = null;
            bool result = false;
            
            if (_mEventRaised)
            {
                nextState = _mNextState;
                result = true;
            }
            
            return result;
        }

        public void OnEventRaised()
        {
            _mEventRaised = true;
        }

        public void Enable()
        {

            _mGameEvent += OnEventRaised;
            _mEventRaised = false;
        }
        
        public void Disable()
        {
            _mGameEvent -= OnEventRaised;
            _mEventRaised = false;
        }
    }
}