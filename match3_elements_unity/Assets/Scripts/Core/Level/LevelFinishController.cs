using Core.Grid;
using Zenject;

namespace Core.Level
{
	public sealed class LevelFinishController
	{
		private readonly INextLevelLoader nextLevelLoader;

		[Inject]
		public LevelFinishController(
			INextLevelLoader nextLevelLoader, 
			IBlocksOnGridFieldClearedEvent gridFieldClearedEvent)
		{
			this.nextLevelLoader = nextLevelLoader;
			
			gridFieldClearedEvent.OnBlocksOnGridFiledCleared += OnBlocksOnGridFiledCleared;
		}

		private void OnBlocksOnGridFiledCleared()
			=> nextLevelLoader.LoadNextLevel();
	}
}