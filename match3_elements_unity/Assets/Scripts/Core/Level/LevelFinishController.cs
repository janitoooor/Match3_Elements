using Core.BlocksSwipe;
using Zenject;

namespace Core.Level
{
	public sealed class LevelFinishController
	{
		private readonly INextLevelLoader nextLevelLoader;

		[Inject]
		public LevelFinishController(
			INextLevelLoader nextLevelLoader, 
			IAllBlocksOnGridKilledEvent gridKilledEvent)
		{
			this.nextLevelLoader = nextLevelLoader;
			
			gridKilledEvent.OnAllBlocksOnGridKilled += OnAllBlocksOnGridKilled;
		}

		private void OnAllBlocksOnGridKilled()
			=> nextLevelLoader.LoadNextLevel();
	}
}