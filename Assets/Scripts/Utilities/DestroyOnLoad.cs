using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Utilities
{
    /// <summary>
    /// This component marks temporary objects in the scene that can be destroyed when
    /// loading into another scene.
    ///
    /// This is useful when loading gameplay scenes additively. Attach this to any GameObject
    /// only needed for testing (e.g. cameras) but not necessary when loading into another scene.
    /// </summary>
    public class DestroyOnLoad : MonoBehaviour
    {
        // Inspector fields
        [FormerlySerializedAs("m_ActiveWithinScene")]
        [Tooltip("Don't destroy the GameObject when this is the active scene")]
        [SerializeField] private string mActiveWithinScene;
        [FormerlySerializedAs("m_ObjectToDestroy")]
        [Tooltip("Defaults to current GameObject unless specified")]
        [SerializeField] private GameObject mObjectToDestroy;
        [FormerlySerializedAs("m_Debug")]
        [Tooltip("Shows debug messages.")]
        [SerializeField] private bool mDebug;

        private void Start()
        {
            if (SceneManager.GetActiveScene().name != mActiveWithinScene)
            {
                if (mObjectToDestroy == null)
                    mObjectToDestroy = gameObject;

                Destroy(mObjectToDestroy);

                if (mDebug)
                {
                    Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);
                    Debug.Log("Do not destroy in scene: " + mActiveWithinScene);
                    Debug.Log("Destroy on load: " + mObjectToDestroy);
                }
            }
        }
    }
}
