using System.Collections;
using UnityEngine.SceneManagement;

namespace Base
{
	public delegate void OnSceneLoadedDelegate();
	public delegate void OnSceneLoadProgressFinishedDelegate();
	public delegate void OnSceneLoadProgressUpdatedDelegate(float progress);
	
	/// <summary>
	/// Used for load scenes in game by different methods
	/// </summary>
	public interface ISceneLoader
	{
		event OnSceneLoadProgressFinishedDelegate OnSceneLoadProgressFinished;
		event OnSceneLoadProgressUpdatedDelegate OnSceneLoadProgressUpdated;
		void SetActiveScene(LoadedSceneType loadedSceneType);
		public IEnumerator LoadSceneAsync(LoadedSceneType singleLoadedSceneType, LoadSceneMode loadSceneMode, 
			float maxProgress = 1f, bool allowSceneActivation = false, OnSceneLoadedDelegate loadedCallback = null);
		void ActivateLoadedAsyncScenes();
	}
}