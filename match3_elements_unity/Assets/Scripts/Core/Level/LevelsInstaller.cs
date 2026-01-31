using Core.Animation.Configs;
using Core.Blocks;
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
		private AnimationsContainer animationsContainer;
		
		[SerializeField]
		private GridField gridField;
		
		public override void InstallBindings()
		{
			Container.Bind<ILevelsContainer>().FromInstance(levelsContainer).AsSingle();

			Container.Bind<LevelAsyncDataInitializer>().AsSingle().NonLazy();
			
			Container.Bind<ICurrentLevelProvider>().To<CurrentLevelProvider>().AsSingle();
			Container.Bind<ILevelConstructor>().To<LevelConstructor>().AsSingle();
			
			Container.Bind<IAnimationsContainer>().FromInstance(animationsContainer).AsSingle();
			
			Container.BindInterfacesTo<GridField>().FromInstance(gridField).AsSingle();
			
			Container.Bind<IBlocksGenerator>().To<BlocksGenerator>().AsSingle();
			
			Container.Bind<IBlocksOnGridFieldProvider>().To<BlocksOnGridFieldProvider>().AsSingle();
			Container.Bind<IBlocksOnGridSwipeModel>().To<BlocksOnGridSwipeModel>().AsSingle();
			
			Container.Bind<BlocksSwipeInputController>().AsSingle().NonLazy();
		}
	}
}