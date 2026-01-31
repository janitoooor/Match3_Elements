using Core.Level.Configs;
using Zenject;

namespace Core.Level
{
	public sealed class CurrentLevelDataProvider : ICurrentLevelDataProvider
	{
		private readonly ILevelsContainer levelsContainer;
		private readonly ICurrentLevelProvider currentLevelProvider;

		[Inject]
		public CurrentLevelDataProvider(ILevelsContainer levelsContainer, ICurrentLevelProvider currentLevelProvider)
		{
			this.levelsContainer = levelsContainer;
			this.currentLevelProvider = currentLevelProvider;
		}

		public ILevelData GetCurrentLevelData()
			=> levelsContainer.GetLevelData(currentLevelProvider.currentLevel);
	}
}