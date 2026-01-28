using System;
using Base.Gui;
using UnityEngine.SceneManagement;
using Zenject;

namespace Base
{
	public sealed class GameRegimeLoader : IGameRegimeLoader
	{
		private const float COMMON_SCENE_LOAD_PROGRESS = 0.2F;
		private const float REGIME_SCENE_UNLOAD_PROGRESS = 0.2F;
		private const float REGIME_SCENE_LOAD_PROGRESS = 0.4F;
		private const float REGIME_ACTIVATE_LOAD_PROGRESS = 0.4F;
		
		private readonly IGuiEngine guiEngine;
		private readonly ISceneLoadProvider sceneLoadProvider;
		private readonly IGameRegimeActivator regimeActivator; 

		private GameRegime? currentGameRegime;
		
		[Inject]
		public GameRegimeLoader(
			IGuiEngine guiEngine, 
			ISceneLoadProvider sceneLoadProvider, 
			IGameRegimeActivator regimeActivator)
		{
			this.guiEngine = guiEngine;
			this.sceneLoadProvider = sceneLoadProvider;
			this.regimeActivator = regimeActivator;
			
			sceneLoadProvider.OnProgressUpdated += OnRegimeProgressUpdated;
			regimeActivator.OnProgressUpdated += OnRegimeProgressUpdated;
		}

		public void LoadDefaultRegime()
		{
			guiEngine.ShowProgressLoadingView();
			LoadCommonScene();
		}

		public void LoadRegime(GameRegime gameRegime)
		{
			guiEngine.ShowProgressLoadingView();
			
			if (currentGameRegime != null)
				TryUnloadCurrentGameRegime(()=> SetCurrentRegimeAndLoad(gameRegime));
			else
				SetCurrentRegimeAndLoad(gameRegime);
		}
		
		private void TryUnloadCurrentGameRegime(SceneUnLoadedDelegate unLoadedCallback)
			=> sceneLoadProvider.UnloadSceneAsync(
				GetRegimeScene(currentGameRegime), 
				REGIME_SCENE_UNLOAD_PROGRESS, 
				unLoadedCallback);

		private void LoadCommonScene()
			=> sceneLoadProvider.LoadSceneAsync(
				LoadedSceneType.Common, 
				LoadSceneMode.Additive, 
				COMMON_SCENE_LOAD_PROGRESS, 
				OnCommonSceneLoaded);

		private void OnCommonSceneLoaded()
			=> SetCurrentRegimeAndLoad(GetDefaultRegime());

		private void SetCurrentRegimeAndLoad(GameRegime gameRegime)
		{
			currentGameRegime = gameRegime;
			LoadCurrentGameRegimeScene();
		}

		private void LoadCurrentGameRegimeScene()
		{
			var regimeScene = GetRegimeScene(currentGameRegime);
			
			sceneLoadProvider.LoadSceneAsync(
				regimeScene, 
				LoadSceneMode.Additive,
				REGIME_SCENE_LOAD_PROGRESS,
				LoadedRegimeSceneCallback(regimeScene));
		}

		private SceneLoadedDelegate LoadedRegimeSceneCallback(LoadedSceneType regimeScene)
			=> ()=>
			{
				sceneLoadProvider.SetActiveScene(regimeScene);
				regimeActivator.ActivateRegime(REGIME_ACTIVATE_LOAD_PROGRESS, () => guiEngine.HideProgressLoadingView());
			};
		
		private void OnRegimeProgressUpdated(float progress)
			=> guiEngine.UpdateProgressLoadingView(progress);
		
		private static LoadedSceneType GetRegimeScene(GameRegime? gameRegime)
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