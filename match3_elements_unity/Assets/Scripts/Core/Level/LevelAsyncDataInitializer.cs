using System.Collections;
using Base;
using Core.Enums;
using Core.Level.Configs;
using Zenject;

namespace Core.Level
{
	public sealed class LevelAsyncDataInitializer : AsyncDataInitializer
	{
		private readonly ILevelsContainer levelsContainer;
		private readonly ICurrentLevelProvider currentLevelProvider;
		private readonly ILevelConstructor levelConstructor;

		[Inject]
		public LevelAsyncDataInitializer(
			ILevelsContainer levelsContainer,
			ICurrentLevelProvider currentLevelProvider, 
			ILevelConstructor levelConstructor)
		{
			this.levelsContainer = levelsContainer;
			this.currentLevelProvider = currentLevelProvider;
			this.levelConstructor = levelConstructor;
		}

		public override int priority => (int)CoreAsyncDataInitializePriority.LevelInitialization;
		
		public override IEnumerator Initialize()
		{
			yield return levelConstructor.ConstructLevel(GetCurrentLevelData());
		}

		private ILevelData GetCurrentLevelData()
			=> levelsContainer.GetLevelData(currentLevelProvider.currentLevel);
	}
}