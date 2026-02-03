using Base;
using Base.Gui.Enums;
using Common.Saves;
using Zenject;

namespace Meta.MainWidget
{
	public sealed class MetaMainWidgetModel : IMetaMainWidgetModel
	{
		private readonly IGameRegimeLoader gameRegimeLoader;
		private readonly ISavesWriter savesWriter;

		[Inject]
		public MetaMainWidgetModel(IGameRegimeLoader gameRegimeLoader, ISavesWriter savesWriter)
		{
			this.gameRegimeLoader = gameRegimeLoader;
			this.savesWriter = savesWriter;
		}

		public void HandleButtonClick(WidgetButtonType buttonType)
		{
			switch (buttonType)
			{
				case WidgetButtonType.ClearSaves:
					savesWriter.DeleteAllSaves();
					break;
				case WidgetButtonType.StartGame:
					gameRegimeLoader.LoadRegime(GameRegime.Core);
					break;
			}
		}
	}
}