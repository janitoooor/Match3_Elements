using UnityEngine.SceneManagement;

namespace Base
{
	public delegate void OnSceneLoadedDelegate();
	public delegate void OnSceneLoadProgressFinishedDelegate();
	
	/// <summary>
	/// Used for load scenes in game by different methods
	/// </summary>
	public interface ISceneLoader : IProgressUpdatable
	{
		event OnSceneLoadProgressFinishedDelegate OnSceneLoadProgressFinished;
		void SetActiveScene(LoadedSceneType loadedSceneType);
		public void LoadSceneAsync(LoadedSceneType singleLoadedSceneType, LoadSceneMode loadSceneMode, 
			float maxProgress = 1f, bool allowSceneActivation = false, OnSceneLoadedDelegate loadedCallback = null);
		void ActivateLoadedAsyncScenes();
	}
}