using Core.Blocks;
using Core.BlocksMovements;
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
			
			Container.Bind<IBlocksOnGridFieldProvider>().To<BlocksOnGridFieldProvider>().AsSingle();
		}

		private void BindLevel()
		{
			Container.Bind<NewLevelBlocksOnGridShowGameRegimeSyncStartAction>().AsSingle().NonLazy();
			
			Container.Bind<ICurrentLevelDataProvider>().To<CurrentLevelDataProvider>().AsSingle();
			
			Container.Bind<ILevelsContainer>().FromInstance(levelsContainer).AsSingle();

			Container.Bind<LevelAsyncDataInitializer>().AsSingle().NonLazy();
			
			Container.Bind<ICurrentLevelProvider>().To<CurrentLevelProvider>().AsSingle();
			Container.Bind<ILevelConstructor>().To<LevelConstructor>().AsSingle();
		}

		private void BindBlockSwipe()
		{
			Container.Bind<IBlocksOnGridSwipeModel>().To<BlocksOnGridSwipeModel>().AsSingle();

			Container.Bind<BlocksSwipeInputController>().AsSingle().NonLazy();
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