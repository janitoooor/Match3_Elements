using Common.Saves;
using Core.Level.Configs;
using Core.Saves;
using Zenject;

namespace Core.Level
{
	public sealed class CurrentLevelChanger : ICurrentLevelChanger
	{
		private readonly ICurrentLevelSavesStorage currentLevelSavesStorage;
		private readonly IBlocksOnGridCoreSavesStorage blocksOnGrid;
		
		private readonly int maxLevel;
		
		[Inject]
		public CurrentLevelChanger(
			ICurrentLevelSavesStorage currentLevelSavesStorage, 
			IBlocksOnGridCoreSavesStorage blocksOnGrid,
			ILevelsContainer levelsContainer)
		{
			this.currentLevelSavesStorage = currentLevelSavesStorage;
			this.blocksOnGrid = blocksOnGrid;
			
			maxLevel = levelsContainer.GetLevelsCount() - 1;
		}

		public void ChangeCurrentLevel()
		{
			blocksOnGrid.ClearLevelData();
			
			var nextLevel = currentLevelSavesStorage.currentLevel + 1;
			currentLevelSavesStorage.currentLevel = nextLevel > maxLevel ? 0 : nextLevel;
		}
	}
}