using System.Collections;
using System.Collections.Generic;
using StateMachine.Interfaces;
using UnityEngine;

namespace StateMachine.States
{
    /// <summary>
    /// An abstract class that provides common functionalities for the states of state machines
    /// </summary>
    public abstract class AbstractState : IState
    {
        /// <summary>
        /// The name of the state used for debugging purposes
        /// </summary>
        public virtual string Name { get; set; }

        // Enable debug messages
        protected bool MDebug = false;
        private readonly List<ILink> _mLinks = new();

        public virtual void Enter()
        {
        }

        public abstract IEnumerator Execute();

        public virtual void Exit()
        {
        }

        public virtual void AddLink(ILink link)
        {
            if (!_mLinks.Contains(link))
            {
                _mLinks.Add(link);
            }
        }

        public virtual void RemoveLink(ILink link)
        {
            if (_mLinks.Contains(link))
            {
                _mLinks.Remove(link);
            }
        }

        public virtual void RemoveAllLinks()
        {
            _mLinks.Clear();
        }

        public virtual bool ValidateLinks(out IState nextState)
        {
            if (_mLinks != null && _mLinks.Count > 0)
            {
                foreach (var link in _mLinks)
                {
                    var result = link.Validate(out nextState);
                    if (result)
                    {
                        return true;
                    }
                }
            }

            //default
            nextState = null;
            return false;
        }

        public void EnableLinks()
        {
            foreach (var link in _mLinks)
            {
                link.Enable();
            }
        }

        public void DisableLinks()
        {
            foreach (var link in _mLinks)
            {
                link.Disable();
            }
        }

        public virtual void DebugState()
        {
            if (MDebug)
                Debug.Log("Current state = " + Name + "(" + this.GetType().Name + ")");
        }
    }
}