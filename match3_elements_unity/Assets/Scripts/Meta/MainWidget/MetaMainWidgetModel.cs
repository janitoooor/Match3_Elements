using Base;
using Zenject;

namespace Meta.MainWidget
{
	public sealed class MetaMainWidgetModel : IMetaMainWidgetModel
	{
		private readonly IGameRegimeLoader gameRegimeLoader;

		[Inject]
		public MetaMainWidgetModel(IGameRegimeLoader gameRegimeLoader)
			=> this.gameRegimeLoader = gameRegimeLoader;

		public void HandleStartButtonClick()
			=> gameRegimeLoader.LoadRegime(GameRegime.Core);
	}
}