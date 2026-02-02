using Base;
using Zenject;

namespace Core.Level
{
	public sealed class NextLevelLoader : INextLevelLoader
	{
		private readonly IGameRegimeLoader gameRegimeLoader;
		private readonly ICurrentLevelProvider currentLevelProvider;

		[Inject]
		public NextLevelLoader(IGameRegimeLoader gameRegimeLoader, ICurrentLevelProvider currentLevelProvider)
		{
			this.gameRegimeLoader = gameRegimeLoader;
			this.currentLevelProvider = currentLevelProvider;
		}

		public void LoadNextLevel()
		{
			currentLevelProvider.ChangeCurrentLevel();
			gameRegimeLoader.RestartCurrentGameRegime();
		}
	}
}