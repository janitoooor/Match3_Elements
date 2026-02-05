using Core.Gui.LevelWinWidget;
using Core.Gui.MainWidget;
using Zenject;

namespace Core.Level
{
	public sealed class LevelFinishFlow : ILevelFinishFlow
	{
		private readonly ILevelWinWidgetProvider levelWinWidgetProvider;
		private readonly ICoreWidgetProvider coreMainWidgetProvider;
		private readonly ICurrentLevelChanger currentLevelChanger;

		[Inject]
		public LevelFinishFlow(
			ILevelWinWidgetProvider levelWinWidgetProvider, 
			ICurrentLevelChanger currentLevelChanger,
			ICoreWidgetProvider coreMainWidgetProvider)
		{
			this.levelWinWidgetProvider = levelWinWidgetProvider;
			this.coreMainWidgetProvider = coreMainWidgetProvider;
			this.currentLevelChanger = currentLevelChanger;
		}
		
		public void FinishLevel()
		{
			currentLevelChanger.IncreaseCurrentLevel();
			coreMainWidgetProvider.HideButtons();
			levelWinWidgetProvider.ShowWidget();
		}
	}
}