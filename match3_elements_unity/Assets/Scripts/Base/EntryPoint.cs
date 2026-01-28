using System.Collections;
using Base.Gui;
using UnityEngine;
using Zenject;

namespace Base
{
	/// <summary>
	/// Entry point for dependency injection setup using Zenject.
	/// Configures and initializes core game systems and dependencies.
	/// This installer creates the main game entity responsible for coordinating
	/// the core gameplay loop and system initialization.
	/// </summary>
	public sealed class EntryPoint : MonoInstaller, IAsyncProcessor
	{
		[Tooltip("Specifies an entity which may be used to manage the game GUI."), SerializeField]
		private GuiEngine guiEngine;
		
		public override void InstallBindings()
		{
			Container.Bind<IAsyncProcessor>().FromInstance(this);
			
			Container.Bind<ISceneLoader>().To<UnitySceneManagementSceneLoader>().AsSingle();
			
			Container.Bind<IGuiEngine>().FromInstance(guiEngine);
			
			Container.Bind<IGameRegimeLoader>().To<GameRegimeLoader>().AsSingle();
			
			Container.Bind<IGameRegimeActivator>().To<GameRegimeActivator>().AsSingle();
			
			Container.BindInterfacesTo<AsyncDataInitializerChain>().AsSingle();
			Container.BindInterfacesTo<GameRegimeSyncStartActionChain>().AsSingle();
		}

		public override void Start()
		{
			base.Start();
			
			Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;

			StartCoroutine(StartGameRoutine());
		}

		private IEnumerator StartGameRoutine()
		{
			yield return null;

			TryStartGame();
		}
		
		private void TryStartGame()
		{
			var game = Container.Instantiate<Game>();
		
			Container.Bind<IGame>().FromInstance(game);
			
			game.Start();
		}
	}
}