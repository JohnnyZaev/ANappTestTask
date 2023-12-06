using System;
using System.Collections;
using UnityEngine;

namespace Utilities
{
    /// <summary>
    ///  The Coroutines class provides static methods for managing coroutines
    ///  without the need for a MonoBehaviour component attached to a game object.
    ///  This is useful if you have a ScriptableObject that needs to run a coroutine.
    /// </summary>
    public static class Coroutines
    {
        private static MonoBehaviour _sCoroutineRunner;

        public static bool IsInitialized => _sCoroutineRunner != null;

        public static void Initialize(MonoBehaviour runner)
        {
            _sCoroutineRunner = runner;
        }

        public static Coroutine StartCoroutine(IEnumerator coroutine)
        {
            if (_sCoroutineRunner == null)
            {
                throw new InvalidOperationException("CoroutineRunner is not initialized.");
            }

            return _sCoroutineRunner.StartCoroutine(coroutine);
        }

        public static void StopCoroutine(Coroutine coroutine)
        {
            if (_sCoroutineRunner != null)
            {
                _sCoroutineRunner.StopCoroutine(coroutine);
            }
        }

        public static void StopCoroutine(ref Coroutine coroutine)
        {
            if (_sCoroutineRunner != null && coroutine != null)
            {
                _sCoroutineRunner.StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }
}
