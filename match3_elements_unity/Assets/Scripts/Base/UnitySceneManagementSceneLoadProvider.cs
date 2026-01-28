using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Base
{
    /// <summary>
    /// Unity SceneManager wrapper for scene loading operations.
    /// Provides both synchronous and asynchronous scene loading with event notifications.
    /// Implements proper resource management and progress tracking for async operations.
    /// </summary>
    public sealed class UnitySceneManagementSceneLoadProvider : ISceneLoadProvider
    {
        public event ProgressUpdatedDelegate OnProgressUpdated;

        private readonly IAsyncProcessor asyncProcessor;
        
        private AsyncOperation currentAsyncLoadOperation;
        
        [Inject]
        public UnitySceneManagementSceneLoadProvider(IAsyncProcessor asyncProcessor)
            => this.asyncProcessor = asyncProcessor;

        public void SetActiveScene(LoadedSceneType loadedSceneType)
            => SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadedSceneType.ToString()));

        public void UnloadSceneAsync(LoadedSceneType unLoadedSceneType, float maxProgress = 1f, 
            SceneUnLoadedDelegate unLoadedCallback = null)
        {
            if (CheckIsSceneExists(unLoadedSceneType, out var sceneName))
                asyncProcessor.StartCoroutine(UnloadSceneAsyncInternal(sceneName, maxProgress, unLoadedCallback));
        }

        private IEnumerator UnloadSceneAsyncInternal(string sceneName, float maxProgress, SceneUnLoadedDelegate unLoadedCallback)
        {
            var currentAsyncOperation = SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

            if (currentAsyncOperation == null)
                yield break;
            
            yield return UpdateProgressRoutine(maxProgress, currentAsyncOperation);

            unLoadedCallback?.Invoke();
        }
        
        private static float CalculateDeltaProgress(float maxProgress, float deltaProgress, float asyncOperationProgress)
            => asyncOperationProgress * maxProgress - deltaProgress;

        public void LoadSceneAsync(LoadedSceneType loadedSceneType, LoadSceneMode loadSceneMode,
            float maxProgress = 1f, SceneLoadedDelegate loadedCallback = null)
        {
            if (CheckIsSceneExists(loadedSceneType, out var sceneName))
                asyncProcessor.StartCoroutine(LoadSceneAsyncInternal(
                    sceneName, 
                    loadSceneMode, 
                    maxProgress, 
                    loadedCallback));
        }
        
        private static string GetSceneName(LoadedSceneType loadedSceneType)
            => loadedSceneType.ToString();

        private static bool CheckIsSceneExists(LoadedSceneType loadedSceneType, out string sceneName)
        {
            sceneName = GetSceneName(loadedSceneType);
            
            if (SceneExists(sceneName))
                return true;
            
            Debug.LogError($"Scene '{sceneName}' not found in build settings!");
            return false;
        }

        private IEnumerator LoadSceneAsyncInternal(string sceneName, LoadSceneMode loadSceneMode, float maxProgress, 
            SceneLoadedDelegate loadedCallback)
        {
            currentAsyncLoadOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            
            if (currentAsyncLoadOperation == null)
                yield break;
            
            currentAsyncLoadOperation.allowSceneActivation = true;

            yield return UpdateProgressRoutine(maxProgress, currentAsyncLoadOperation);
            
            currentAsyncLoadOperation = null;
            
            loadedCallback?.Invoke();
        }
        
        private IEnumerator UpdateProgressRoutine(float maxProgress, AsyncOperation currentAsyncOperation)
        {
            var deltaProgress = 0f;

            while (!currentAsyncOperation.isDone)
            {
                yield return null;

                deltaProgress = CalculateDeltaProgress(maxProgress, deltaProgress, currentAsyncOperation.progress);
                
                OnProgressUpdated?.Invoke(deltaProgress);
            }
        }
        
        public void ActivateLoadedAsyncScenes()
        {
            if (currentAsyncLoadOperation != null)
                currentAsyncLoadOperation.allowSceneActivation = true;
        }
        
        private static bool SceneExists(string sceneName)
        {
            for (var i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                var name = System.IO.Path.GetFileNameWithoutExtension(scenePath);
         
                if (name.Equals(sceneName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}