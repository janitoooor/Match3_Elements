using Core.Level.Configs;
using Zenject;

namespace Core.Level
{
	public sealed class CurrentLevelProvider : ICurrentLevelProvider
	{
		private static int staticLevel;

		private readonly int maxLevel;
		
		public int currentLevel => staticLevel;

		[Inject]
		public CurrentLevelProvider(ILevelsContainer levelsContainer)
			=> maxLevel = levelsContainer.GetLevelsCount() - 1;

		public void ChangeCurrentLevel()
		{
			var nexLevel = staticLevel + 1;
			staticLevel = nexLevel > maxLevel ? 0 : nexLevel;
		}
	}
}