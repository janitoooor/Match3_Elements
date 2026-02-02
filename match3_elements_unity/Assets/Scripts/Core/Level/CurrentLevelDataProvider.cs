using Common.Saves;
using Core.Level.Configs;
using Zenject;

namespace Core.Level
{
	public sealed class CurrentLevelDataProvider : ICurrentLevelDataProvider
	{
		private readonly ILevelsContainer levelsContainer;
		private readonly ICurrentLevelSavesStorage currentLevelSavesStorage;

		[Inject]
		public CurrentLevelDataProvider(ILevelsContainer levelsContainer, ICurrentLevelSavesStorage currentLevelSavesStorage)
		{
			this.levelsContainer = levelsContainer;
			this.currentLevelSavesStorage = currentLevelSavesStorage;
		}

		public ILevelData GetCurrentLevelData()
			=> levelsContainer.GetLevelData(currentLevelSavesStorage.currentLevel);
	}
}