using Core.Enums;
using Core.Level;
using Zenject;

namespace Core.MainWidget
{
	public sealed class CoreMainWidgetModel : ICoreMainWidgetModel
	{
		private readonly ILevelRestarter levelRestarter;
		private readonly INextLevelLoader nextLevelLoader;

		[Inject]
		public CoreMainWidgetModel(ILevelRestarter levelRestarter, INextLevelLoader nextLevelLoader)
		{
			this.levelRestarter = levelRestarter;
			this.nextLevelLoader = nextLevelLoader;
		}

		public void HandleButtonClick(CoreMainWidgetButtonType buttonType)
		{
			if (buttonType == CoreMainWidgetButtonType.NextLevel)
				nextLevelLoader.LoadNextLevel();
			else
				levelRestarter.RestartLevel();
		}
	}
}