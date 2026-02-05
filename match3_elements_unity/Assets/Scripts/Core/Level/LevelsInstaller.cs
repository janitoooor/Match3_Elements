using Core.Blocks;
using Core.BlocksKill;
using Core.BlocksMovements;
using Core.BlocksSwipe;
using Core.Grid;
using Core.Level.Configs;
using UnityEngine;
using Zenject;

namespace Core.Level
{
	public sealed class LevelsInstaller : MonoInstaller
	{
		[SerializeField]
		private LevelsContainer levelsContainer;
		
		[SerializeField]
		private BlocsContainer blocsContainer;
		
		[SerializeField]
		private GridField gridField;
		
		public override void InstallBindings()
		{
			BindLevel();

			BindGridFieldWithBlocks();

			BindBlockSwipe();

			BindBlockMovements();
		}

		private void BindGridFieldWithBlocks()
		{
			Container.Bind<IBlocsContainer>().FromInstance(blocsContainer).AsSingle();
			
			Container.Bind<IGridField>().To<GridField>().FromInstance(gridField).AsSingle();
			
			Container.Bind<IBlocksGenerator>().To<BlocksGenerator>().AsSingle();
			
			Container.Bind<IBlocksOnGridRepository>().To<BlocksOnGridRepository>().AsSingle();
			Container.Bind<IBlocksOnGridConstructor>().To<BlocksOnGridConstructor>().AsSingle();
			Container.Bind<IBlockOnGridRendererSortingOderProvider>().To<BlocksOnGridRendererSortingOderProvider>().AsSingle();
		}

		private void BindLevel()
		{
			Container.Bind<NewLevelBlocksOnGridShowGameRegimeSyncStartAction>().AsSingle().NonLazy();
			Container.Bind<NewLevelBlocksOnGridFallGameRegimeSyncStartAction>().AsSingle().NonLazy();
			Container.Bind<IBlockOnGridFieldRequestKillEvent>().To<NewLevelBlocksOnGridKillGameRegimeSyncStartAction>().AsSingle();
			
			Container.Bind<ICurrentLevelDataProvider>().To<CurrentLevelDataProvider>().AsSingle();
			
			Container.Bind<ILevelsContainer>().FromInstance(levelsContainer).AsSingle();

			Container.Bind<LevelAsyncDataInitializer>().AsSingle().NonLazy();
			
			Container.Bind<ICurrentLevelChanger>().To<CurrentLevelChanger>().AsSingle();
			Container.Bind<ILevelConstructor>().To<LevelConstructor>().AsSingle();
			
			Container.Bind<LevelFinishController>().AsSingle().NonLazy();
			Container.Bind<INextLevelLoader>().To<NextLevelLoader>().AsSingle();
			Container.Bind<ILevelRestarter>().To<LevelRestarter>().AsSingle();
			
			Container.Bind<ILevelFinishFlow>().To<LevelFinishFlow>().AsSingle();
		}

		private void BindBlockSwipe()
		{
			Container.Bind<IBlocksOnGridSwipeModel>().To<BlocksOnGridSwipeModel>().AsSingle();

			Container.Bind<BlocksSwipeInputController>().AsSingle().NonLazy();
			
			Container.BindInterfacesTo<BlocksOnGridFieldMover>().AsSingle();
			
			BindBlocksKills();
		}

		private void BindBlocksKills()
		{
			Container.BindInterfacesTo<BlocksOnGridFieldKiller>().AsSingle();
			Container.Bind<BlockOnGridFieldRequestKillController>().AsSingle().NonLazy();
			Container.Bind<IBlockOnGridFieldRequestKillHandler>().To<BlockOnGridFieldRequestKillHandler>().AsSingle();
		}

		private void BindBlockMovements()
		{
			Container.BindInterfacesTo<BlockMovementProcessor>().AsSingle();
			Container.Bind<IBlockMovementFactory>().To<BlockMovementFactory>().AsSingle();
			Container.Bind<IBlockMovementsPool>().To<BlockMovementsPool>().AsSingle();
			Container.Bind<IBlockMovementsPoolCacheSizeCalculator>().To<BlockMovementsPoolCacheSizeCalculator>().AsSingle();
		}
	}
}