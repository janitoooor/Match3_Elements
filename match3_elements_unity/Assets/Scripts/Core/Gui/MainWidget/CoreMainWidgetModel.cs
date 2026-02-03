using Base;
using Base.Gui.Enums;
using Core.Level;
using Zenject;

namespace Core.Gui.MainWidget
{
	public sealed class CoreMainWidgetModel : ICoreMainWidgetModel
	{
		private readonly ILevelRestarter levelRestarter;
		private readonly INextLevelLoader nextLevelLoader;
		private readonly IGameRegimeLoader regimeLoader;

		[Inject]
		public CoreMainWidgetModel(
			ILevelRestarter levelRestarter, 
			INextLevelLoader nextLevelLoader,
			IGameRegimeLoader regimeLoader)
		{
			this.levelRestarter = levelRestarter;
			this.nextLevelLoader = nextLevelLoader;
			this.regimeLoader = regimeLoader;
		}

		public void HandleButtonClick(WidgetButtonType buttonType)
		{
			switch (buttonType)
			{
				case WidgetButtonType.Restart:
					levelRestarter.RestartLevel();
					break;
				case WidgetButtonType.NextLevel:
					nextLevelLoader.LoadNextLevel(true);
					break;
				case WidgetButtonType.MainMenu:
					regimeLoader.LoadRegime(GameRegime.Meta);
					break;
			}
		}
	}
}