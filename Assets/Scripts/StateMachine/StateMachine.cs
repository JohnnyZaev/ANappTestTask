using System;
using System.Collections;
using StateMachine.Interfaces;
using UnityEngine;
using Utilities;

namespace StateMachine
{
    /// <summary>
    /// A Generic state machine, adapted from the Runner template
    /// https://unity.com/features/build-a-runner-game
    /// </summary>
    public class StateMachine
    {
        // The current state the state machine is in
        public IState CurrentState { get; private set; }
        
        /// <summary>
        /// Finalizes the previous state and then runs the new state
        /// </summary>
        /// <param name="state"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual void SetCurrentState(IState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (CurrentState != null && _mCurrentPlayCoroutine != null) 
            {
                //interrupt currently executing state
                Skip();
            }
            
            CurrentState = state;
            Coroutines.StartCoroutine(Play());
        }

        private Coroutine _mCurrentPlayCoroutine;
        private bool _mPlayLock;
        /// <summary>
        /// Runs the life cycle methods of the current state.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Play()
        {
            if (!_mPlayLock)
            {
                _mPlayLock = true;
                
                CurrentState.Enter();

                //keep a ref to execute coroutine of the current state
                //to support stopping it later.
                _mCurrentPlayCoroutine = Coroutines.StartCoroutine(CurrentState.Execute());
                yield return _mCurrentPlayCoroutine;
                
                _mCurrentPlayCoroutine = null;
            }
        }

        /// <summary>
        /// Interrupts the execution of the current state and finalizes it.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void Skip()
        {
            if (CurrentState == null)
                throw new Exception($"{nameof(CurrentState)} is null!");

            if (_mCurrentPlayCoroutine == null) return;
            Coroutines.StopCoroutine(ref _mCurrentPlayCoroutine);
            //finalize current state
            CurrentState.Exit();
            _mCurrentPlayCoroutine = null;
            _mPlayLock = false;
        }
        
        public virtual void Run(IState state)
        {
            SetCurrentState(state);
            Run();
        }

        private Coroutine _mLoopCoroutine;
        /// <summary>
        /// Turns on the main loop of the StateMachine.
        /// This method does not resume previous state if called after Stop()
        /// and the client needs to set the state manually.
        /// </summary>
        public virtual void Run() 
        {
            if (_mLoopCoroutine != null) //already running
                return;
            
            _mLoopCoroutine = Coroutines.StartCoroutine(Loop());
        }

        /// <summary>
        /// Turns off the main loop of the StateMachine
        /// </summary>
        public void Stop()
        {
            if (_mLoopCoroutine == null) //already stopped
                return;
            
            if (CurrentState != null && _mCurrentPlayCoroutine != null) 
            {
                //interrupt currently executing state
                Skip();
            }
            
            Coroutines.StopCoroutine(ref _mLoopCoroutine);
            CurrentState = null;
        }

        /// <summary>
        /// The main update loop of the StateMachine.
        /// It checks the status of the current state and its link to provide state sequencing
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator Loop()
        {
            while (true)
            {
                if (CurrentState != null && _mCurrentPlayCoroutine == null) //current state is done playing
                {
                    if (CurrentState.ValidateLinks(out var nextState))
                    {
                        if (_mPlayLock)
                        {
                            //finalize current state
                            CurrentState.Exit();
                            _mPlayLock = false;
                        }
                        CurrentState.DisableLinks();
                        SetCurrentState(nextState);
                        CurrentState.EnableLinks();
                    }
                }

                yield return null;
            }
        }

        public bool IsRunning => _mLoopCoroutine != null;
    }
}
