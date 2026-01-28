using System;
using Base.Gui;
using UnityEngine.SceneManagement;
using Zenject;

namespace Base
{
	public sealed class GameRegimeLoader : IGameRegimeLoader
	{
		private const float COMMON_SCENE_LOAD_PROGRESS = 0.1F;
		private const float REGIME_SCENE_LOAD_PROGRESS = 0.4F;
		private const float REGIME_ACTIVATE_LOAD_PROGRESS = 0.5F;
		
		private readonly ISceneLoader sceneLoader;
		private readonly IGuiEngine guiEngine;
		private readonly IGameRegimeActivator regimeActivator; 

		private GameRegime currentGameRegime;
		
		[Inject]
		public GameRegimeLoader(
			ISceneLoader sceneLoader, 
			IGuiEngine guiEngine, 
			IGameRegimeActivator regimeActivator)
		{
			this.sceneLoader = sceneLoader;
			this.guiEngine = guiEngine;
			this.regimeActivator = regimeActivator;
			
			sceneLoader.OnProgressUpdated += OnRegimeProgressUpdated;
		}

		public void LoadDefaultRegime()
		{
			guiEngine.ShowProgressLoadingView();
			LoadCommonScene();
		}

		private void LoadCommonScene()
			=> sceneLoader.LoadSceneAsync(
				LoadedSceneType.Common, 
				LoadSceneMode.Additive, 
				COMMON_SCENE_LOAD_PROGRESS, 
				true,
				OnCommonSceneLoaded);

		private void OnCommonSceneLoaded()
		{
			sceneLoader.OnSceneLoadProgressFinished += OnRegimeSceneLoadProgressFinished;

			currentGameRegime = GetDefaultRegime();

			LoadCurrentGameRegimeScene();
		}

		private void LoadCurrentGameRegimeScene()
		{
			var regimeScene = GetRegimeScene(currentGameRegime);
			
			sceneLoader.LoadSceneAsync(
				regimeScene, 
				LoadSceneMode.Additive,
				REGIME_SCENE_LOAD_PROGRESS,
				false,
				LoadedRegimeSceneCallback(regimeScene));
		}

		private OnSceneLoadedDelegate LoadedRegimeSceneCallback(LoadedSceneType regimeScene)
			=> ()=>
			{
				sceneLoader.SetActiveScene(regimeScene);
				regimeActivator.ActivateRegime(REGIME_ACTIVATE_LOAD_PROGRESS, () => guiEngine.HideProgressLoadingView());
			};

		private void OnRegimeSceneLoadProgressFinished()
			=> sceneLoader.ActivateLoadedAsyncScenes();
		
		private void OnRegimeProgressUpdated(float progress)
			=> guiEngine.UpdateProgressLoadingView(progress);
		
		private static LoadedSceneType GetRegimeScene(GameRegime gameRegime)
		{
			return gameRegime switch
			{
				GameRegime.Meta => LoadedSceneType.Meta,
				GameRegime.Core => LoadedSceneType.Core,
				_ => throw new ArgumentOutOfRangeException(nameof(gameRegime), gameRegime, null)
			};
		}
		
		private GameRegime GetDefaultRegime()
			=> GameRegime.Meta;
	}
}