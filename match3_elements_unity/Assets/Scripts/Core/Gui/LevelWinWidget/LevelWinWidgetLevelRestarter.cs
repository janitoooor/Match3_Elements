using Core.Level;
using Zenject;

namespace Core.Gui.LevelWinWidget
{
	public sealed class LevelWinWidgetLevelRestarter : ILevelWinWidgetLevelRestarter
	{
		private readonly ILevelRestarter levelRestarter;
		private readonly ICurrentLevelChanger currentLevelChanger;

		[Inject]
		public LevelWinWidgetLevelRestarter(
			ILevelRestarter levelRestarter, 
			ICurrentLevelChanger currentLevelChanger)
		{
			this.levelRestarter = levelRestarter;
			this.currentLevelChanger = currentLevelChanger;
		}

		public void RestartLevel()
		{
			currentLevelChanger.DecreaseCurrentLevel();
			levelRestarter.RestartLevel();
		}
	}
}