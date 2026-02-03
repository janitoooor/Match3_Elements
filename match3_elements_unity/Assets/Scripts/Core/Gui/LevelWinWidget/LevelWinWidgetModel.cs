using Base;
using Base.Gui.Enums;
using Core.Level;
using Zenject;

namespace Core.Gui.LevelWinWidget
{
	public sealed class LevelWinWidgetModel : ILevelWinWidgetModel
	{
		private readonly ILevelRestarter levelRestarter;
		private readonly INextLevelLoader nextLevelLoader;
		private readonly IGameRegimeLoader gameRegimeLoader;

		[Inject]
		public LevelWinWidgetModel(
			ILevelRestarter levelRestarter, 
			INextLevelLoader nextLevelLoader,
			IGameRegimeLoader gameRegimeLoader)
		{
			this.levelRestarter = levelRestarter;
			this.nextLevelLoader = nextLevelLoader;
			this.gameRegimeLoader = gameRegimeLoader;
		}

		public void HandleButtonClick(WidgetButtonType buttonType)
		{
			switch (buttonType)
			{
				case WidgetButtonType.Restart:
					levelRestarter.RestartLevel();
					break;
				case WidgetButtonType.NextLevel:
					nextLevelLoader.LoadNextLevel(false);
					break;
				case WidgetButtonType.MainMenu:
					gameRegimeLoader.LoadRegime(GameRegime.Meta);
					break;
			}
		}
	}
}