using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    /// <summary>
    /// Use this basic helper for loading scenes by name, index, etc.
    /// </summary>

    public class SceneLoader : MonoBehaviour
    {
        // Default loaded scene that serves as the entry point and does not unload
        private Scene _mBootstrapScene;

        // The previously loaded scene
        private Scene _mLastLoadedScene;

        // Properties
        public Scene BootstrapScene => _mBootstrapScene;

        // MonoBehaviour event functions

        private void Start()
        {
            // Set scene 0 as the Bootloader/Bootstrapscene
            _mBootstrapScene = SceneManager.GetActiveScene();
        }

        private void OnEnable()
        {
            SceneEvents.LoadSceneByPath += SceneEvents_LoadSceneByPath;
            SceneEvents.UnloadSceneByPath += SceneEvents_UnloadSceneByPath;
            SceneEvents.SceneIndexLoaded += SceneEvents_SceneIndexLoaded;
            SceneEvents.LastSceneUnloaded += SceneEvents_LastSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneEvents.LoadSceneByPath -= SceneEvents_LoadSceneByPath;
            SceneEvents.UnloadSceneByPath -= SceneEvents_UnloadSceneByPath;
            SceneEvents.SceneIndexLoaded -= SceneEvents_SceneIndexLoaded;
            SceneEvents.LastSceneUnloaded -= SceneEvents_LastSceneUnloaded;
        }

        // Event-handling methods

        private void SceneEvents_LastSceneUnloaded()
        {
            UnloadLastLoadedScene();
        }

        private void SceneEvents_SceneIndexLoaded(int sceneIndex)
        {
            LoadSceneAdditively(sceneIndex);
        }

        private void SceneEvents_LoadSceneByPath(string scenePath)
        {
            LoadSceneByPath(scenePath);
        }

        private void SceneEvents_UnloadSceneByPath(string scenePath)
        {
            UnloadSceneByPath(scenePath);
        }

        // Methods

        // Load a scene non-additively
        public void LoadSceneByPath(string scenePath)
        {
            StartCoroutine(LoadScene(scenePath));
        }

        // Coroutine to unload the previous scene and then load a new scene by scene path string
        private IEnumerator LoadScene(string scenePath)
        {
            if (string.IsNullOrEmpty(scenePath))
            {
                Debug.LogWarning("SceneLoader: Invalid scene name");
                yield break;
            }
            Debug.Log("Loading scenePath: " + scenePath);
            yield return UnloadLastScene();
            yield return LoadAdditiveScene(scenePath);

        }

        // Load a scene by its index number (non-additively)
        public void LoadScene(int buildIndex)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(buildIndex);

            if (string.IsNullOrEmpty(scenePath))
            {
                Debug.LogWarning("SceneLoader.LoadScene: invalid sceneBuildIndex");
                return;
            }

            SceneManager.LoadScene(scenePath);
        }

        // Method to load a scene by its index number (additively)
        public void LoadSceneAdditively(int buildIndex)
        {
            StartCoroutine(LoadAdditiveScene(buildIndex));
        }

        // Reload the current scene
        public void ReloadScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }

        // Load the next scene by index in the Build Settings
        public void LoadNextScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }

        // Unload the last scene (go "back" from a Demo scene)
        public void UnloadLastLoadedScene()
        {
            StartCoroutine(UnloadLastScene());
        }

        // Unload by an explicit path
        private void UnloadSceneByPath(string scenePath)
        {
            Scene sceneToUnload = SceneManager.GetSceneByPath(scenePath);
            if (sceneToUnload.IsValid())
            {
                StartCoroutine(UnloadScene(sceneToUnload));
            }
        }

        // Coroutine to load a scene asynchronously by scene path string in Additive mode,
        // keeps the original scene as the active scene.
        private IEnumerator LoadAdditiveScene(string scenePath)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                float progress = asyncLoad.progress;
                yield return null;
            }

            _mLastLoadedScene = SceneManager.GetSceneByPath(scenePath);
            SceneManager.SetActiveScene(_mLastLoadedScene);
        }

        // Coroutine to load a Scene asynchronously by index in Additive mode,
        // keeps the original scene as the active scene.
        private IEnumerator LoadAdditiveScene(int buildIndex)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(buildIndex);
            yield return LoadAdditiveScene(scenePath);
        }


        // Coroutine to unloads the previously loaded scene if it's not the bootstrap scene
        // Note: this only works for one scene and does not create a breadcrumb list backwards. Use UnloadSceneByPath if
        // needed.
        private IEnumerator UnloadLastScene()
        {
            if (!_mLastLoadedScene.IsValid())
                yield break;

            if (_mLastLoadedScene != _mBootstrapScene)
                yield return UnloadScene(_mLastLoadedScene);
        }

        // Coroutine to unload a specific Scene asynchronously
        private IEnumerator UnloadScene(Scene scene)
        {
            // Break if only have one scene loaded
            if (SceneManager.sceneCount <= 1)
            {
                Debug.Log("[SceneLoader: Cannot unload only loaded scene " + scene.name);
                yield break;
            }

            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(scene);

            while (!asyncUnload.isDone)
            {
                yield return null;
            }
        }

        // Logs the scene path for a single loaded scene
        public static void ShowCurrentScenePath()
        {
            string scenePath = SceneManager.GetActiveScene().path;
            Debug.Log("Current scene path: " + scenePath);
        }

        // Logs the scenes paths for all currently loaded scenes
        public static void ShowAllScenePaths()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                Debug.Log("Scene " + i + " path: " + scene.path);
            }
        }
    }

}

