using Base;
using Core.Enums;
using Core.Level;
using Zenject;

namespace Core.MainWidget
{
	public sealed class CoreMainWidgetModel : ICoreMainWidgetModel
	{
		private readonly IGameRegimeLoader gameRegimeLoader;
		private readonly INextLevelLoader nextLevelLoader;

		[Inject]
		public CoreMainWidgetModel(IGameRegimeLoader gameRegimeLoader, INextLevelLoader nextLevelLoader)
		{
			this.gameRegimeLoader = gameRegimeLoader;
			this.nextLevelLoader = nextLevelLoader;
		}

		public void HandleButtonClick(CoreMainWidgetButtonType buttonType)
		{
			if (buttonType == CoreMainWidgetButtonType.NextLevel)
				nextLevelLoader.LoadNextLevel();
			else
				gameRegimeLoader.RestartCurrentGameRegime();
		}
	}
}