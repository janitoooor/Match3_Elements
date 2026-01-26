using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Base
{
    /// <summary>
    /// Unity SceneManager wrapper for scene loading operations.
    /// Provides both synchronous and asynchronous scene loading with event notifications.
    /// Implements proper resource management and progress tracking for async operations.
    /// </summary>
    public sealed class UnitySceneManagementSceneLoader : ISceneLoader
    {
        public event OnSceneLoadProgressFinishedDelegate OnSceneLoadProgressFinished;
        
        public event OnSceneLoadProgressUpdatedDelegate OnSceneLoadProgressUpdated;
        
        private AsyncOperation currentAsyncOperation;
    
        private bool isLoading;
    
        private const float MAX_SCENE_LOAD_PROGRESS = 0.9f;

        public void SetActiveScene(LoadedSceneType loadedSceneType)
            => SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadedSceneType.ToString()));

        public IEnumerator LoadSceneAsync(LoadedSceneType singleLoadedSceneType, LoadSceneMode loadSceneMode,
            float maxProgress = 1f, bool allowSceneActivation = false, OnSceneLoadedDelegate loadedCallback = null)
        {
            if (!CanLoadScene(singleLoadedSceneType, out var singleSceneName))
                yield break;
            
            yield return LoadSceneAsyncInternal(singleSceneName, loadSceneMode, maxProgress, allowSceneActivation, loadedCallback);
        }
        private bool CanLoadScene(LoadedSceneType singleLoadedSceneType, out string sceneName)
        {
            if (isLoading)
            {
                Debug.LogWarning($"Scene loading already in progress. Aborting async load of {singleLoadedSceneType}");
                sceneName = null;
                return false;
            }
        
            sceneName = singleLoadedSceneType.ToString();
        
            if (!SceneExists(sceneName))
            {
                Debug.LogError($"Scene '{sceneName}' not found in build settings!");
                return false;
            }

            return true;
        }

        private IEnumerator LoadSceneAsyncInternal(string sceneName, LoadSceneMode loadSceneMode, float maxProgress,
            bool allowSceneActivation , OnSceneLoadedDelegate loadedCallback)
        {
            currentAsyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

            if (currentAsyncOperation == null)
                yield break;
        
            isLoading = true;
            
            currentAsyncOperation.allowSceneActivation = allowSceneActivation;
            
            while (!currentAsyncOperation.isDone)
            {
                OnSceneLoadProgressUpdated?.Invoke(CalculateSceneLoadProgress(maxProgress));

                if (currentAsyncOperation.progress >= MAX_SCENE_LOAD_PROGRESS)
                    OnSceneLoadProgressFinished?.Invoke();
                
                yield return null;
            }
            
            currentAsyncOperation = null;
            isLoading = false;
            
            loadedCallback?.Invoke();
        }

        private float CalculateSceneLoadProgress(float maxProgress)
            => Mathf.Clamp(currentAsyncOperation.progress / MAX_SCENE_LOAD_PROGRESS, 0, maxProgress);

        public void ActivateLoadedAsyncScenes()
        {
            if (currentAsyncOperation != null)
                currentAsyncOperation.allowSceneActivation = true;
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