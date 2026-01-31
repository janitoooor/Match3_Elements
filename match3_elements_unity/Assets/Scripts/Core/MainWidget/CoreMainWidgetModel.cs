using Base;
using Core.Enums;
using Core.Level;
using Zenject;

namespace Core.MainWidget
{
	public sealed class CoreMainWidgetModel : ICoreMainWidgetModel
	{
		private readonly IGameRegimeLoader gameRegimeLoader;
		private readonly ICurrentLevelProvider currentLevelProvider;

		[Inject]
		public CoreMainWidgetModel(IGameRegimeLoader gameRegimeLoader, ICurrentLevelProvider currentLevelProvider)
		{
			this.gameRegimeLoader = gameRegimeLoader;
			this.currentLevelProvider = currentLevelProvider;
		}

		public void HandleButtonClick(CoreMainWidgetButtonType buttonType)
		{
			if (buttonType == CoreMainWidgetButtonType.NextLevel)
				currentLevelProvider.ChangeCurrentLevel();
			
			gameRegimeLoader.LoadRegime(GameRegime.Core);
		}
	}
}