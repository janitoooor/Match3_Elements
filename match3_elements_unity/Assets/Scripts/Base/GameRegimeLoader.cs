using System.Collections;
using Base.Gui;
using UnityEngine.SceneManagement;
using Zenject;

namespace Base
{
	public sealed class GameRegimeLoader : IGameRegimeLoader
	{
		private readonly ISceneLoader sceneLoader;
		private readonly IGuiEngine guiEngine;
		private readonly IAsyncProcessor asyncProcessor;

		[Inject]
		public GameRegimeLoader(ISceneLoader sceneLoader, IGuiEngine guiEngine, IAsyncProcessor asyncProcessor)
		{
			this.sceneLoader = sceneLoader;
			this.guiEngine = guiEngine;
			this.asyncProcessor = asyncProcessor;
			
			sceneLoader.OnSceneLoadProgressUpdated += OnRegimeSceneLoadProgressUpdated;
		}

		public void LoadDefaultRegime()
		{
			guiEngine.ShowProgressLoadingView();
			asyncProcessor.StartCoroutine(LoadCommonScene());
		}

		private IEnumerator LoadCommonScene()
			=> sceneLoader.LoadSceneAsync(
				LoadedSceneType.Common, 
				LoadSceneMode.Additive, 
				0.5f, 
				true,
				OnCommonSceneLoaded);

		private void OnCommonSceneLoaded()
		{
			sceneLoader.OnSceneLoadProgressFinished += OnRegimeSceneLoadProgressFinished;
			asyncProcessor.StartCoroutine(LoadRegimeScene(GetDefaultRegimeScene()));
		}

		private IEnumerator LoadRegimeScene(LoadedSceneType loadedSceneType)
		{
			yield return sceneLoader.LoadSceneAsync(
				loadedSceneType, 
				LoadSceneMode.Additive,
				1f,
				false,
				()=> sceneLoader.SetActiveScene(loadedSceneType));
		}

		private void OnRegimeSceneLoadProgressFinished()
		{
			sceneLoader.ActivateLoadedAsyncScenes();
			guiEngine.HideProgressLoadingView();
		}

		private void OnRegimeSceneLoadProgressUpdated(float progress)
			=> guiEngine.UpdateProgressLoadingView(progress);
		
		private LoadedSceneType GetDefaultRegimeScene()
			=> LoadedSceneType.Meta;
	}
}