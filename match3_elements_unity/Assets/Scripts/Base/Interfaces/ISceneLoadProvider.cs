using UnityEngine.SceneManagement;

namespace Base
{
	public delegate void SceneLoadedDelegate();
	public delegate void SceneUnLoadedDelegate();
	
	/// <summary>
	/// Used for load scenes in game by different methods
	/// </summary>
	public interface ISceneLoadProvider : IProgressUpdatable
	{
		void SetActiveScene(LoadedSceneType loadedSceneType);
		public void LoadSceneAsync(LoadedSceneType loadedSceneType, LoadSceneMode loadSceneMode, float maxProgress = 1f,
			SceneLoadedDelegate loadedCallback = null);
		void UnloadSceneAsync(LoadedSceneType unLoadedSceneType, float maxProgress = 1f,
			SceneUnLoadedDelegate unLoadedCallback = null);
	}
}