using Core.BlocksSwipe;
using Zenject;

namespace Core.Level
{
	public sealed class LevelFinishController
	{
		private readonly ILevelFinishFlow levelFinishFlow;

		[Inject]
		public LevelFinishController(
			ILevelFinishFlow levelFinishFlow,
			IAllBlocksOnGridKilledEvent gridKilledEvent)
		{
			this.levelFinishFlow = levelFinishFlow;
			
			gridKilledEvent.OnAllBlocksOnGridKilled += OnAllBlocksOnGridKilled;
		}

		private void OnAllBlocksOnGridKilled()
			=> levelFinishFlow.FinishLevel();
	}
}