using Core.Animation.Configs;
using Core.Blocks;
using Core.CoreCamera;
using Core.Grid;
using Core.Interfaces;
using Core.Level;
using Core.Level.Configs;
using Core.MainWidget;
using Core.SpawnContainer;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Core
{
	public sealed class CoreInstaller : MonoInstaller
	{
		[SerializeField]
		private CorePrefabsContainer corePrefabsContainer;
		
		[SerializeField]
		private LevelsContainer levelsContainer;
		
		[SerializeField]
		private AnimationsContainer animationsContainer;
		
		[SerializeField]
		private GridField gridField;
		
		[FormerlySerializedAs("spawnContainerProvider")]
		[SerializeField]
		private SpawnerInContainer spawnerInContainer;

		public override void InstallBindings()
		{
			BindBaseCoreEntities();

			BindMainWidget();

			BindLevels();
		}

		private void BindLevels()
		{
			Container.Bind<ILevelsContainer>().FromInstance(levelsContainer).AsSingle();

			Container.Bind<LevelAsyncDataInitializer>().AsSingle().NonLazy();
			
			Container.Bind<ICurrentLevelProvider>().To<CurrentLevelProvider>().AsSingle();
			Container.Bind<ILevelConstructor>().To<LevelConstructor>().AsSingle();
			
			Container.Bind<IAnimationsContainer>().FromInstance(animationsContainer).AsSingle();
			
			Container.BindInterfacesTo<GridField>().FromInstance(gridField).AsSingle();
			
			Container.Bind<IBlocksGenerator>().To<BlocksGenerator>().AsSingle();
		}

		private void BindBaseCoreEntities()
		{
#if UNITY_EDITOR
			Container.Bind<ITickable>().To<CoreCameraGridFieldFitGameRegimeSyncStartAction>().AsSingle();
#else
			Container.Bind<CoreCameraGridFieldFitGameRegimeSyncStartAction>().AsSingle().NonLazy();
#endif
			
			Container.Bind<ISpawnerInContainer>().FromInstance(spawnerInContainer).AsSingle();
			
			Container.Bind<ICorePrefabsContainer>().FromInstance(corePrefabsContainer).AsSingle();
		}

		private void BindMainWidget()
		{
			Container.Bind<CoreMainWidgetGameRegimeSyncStartAction>().AsSingle().NonLazy();
			
			Container.BindInterfacesTo<CoreWidgetAsyncDataInitializer>().AsSingle();

			Container.Bind<ICoreMainWidgetModel>().To<CoreMainWidgetModel>().AsSingle();
		}
	}
}