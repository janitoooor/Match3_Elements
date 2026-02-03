using Base;
using Zenject;

namespace Core.Level
{
	public sealed class NextLevelLoader : INextLevelLoader
	{
		private readonly IGameRegimeLoader gameRegimeLoader;
		private readonly ICurrentLevelChanger currentLevelChanger;

		[Inject]
		public NextLevelLoader(IGameRegimeLoader gameRegimeLoader, ICurrentLevelChanger currentLevelChanger)
		{
			this.gameRegimeLoader = gameRegimeLoader;
			this.currentLevelChanger = currentLevelChanger;
		}

		public void LoadNextLevel(bool changeCurrentLevel)
		{
			if (changeCurrentLevel)
				currentLevelChanger.ChangeCurrentLevel();
			
			gameRegimeLoader.RestartCurrentGameRegime();
		}
	}
}