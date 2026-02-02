using Base.Gui;
using Common.MainWidget;
using Meta.Enums;
using Zenject;

namespace Meta.MainWidget
{
	public sealed class MetaWidgetAsyncDataInitializer : WidgetAsyncDataInitialize<
		IMetaPrefabsContainer, 
		MetaPrefabsKeys,
		MetaMainWidget>,
		IMetaWidgetProvider
	{
		private readonly IMetaMainWidgetModel metaMainWidgetModel;

		protected override MetaPrefabsKeys widgetKey => MetaPrefabsKeys.MainWidget;

		public override byte priority => (byte)MetaAsyncDataInitializePriority.MainWidget;
		
		[Inject]
		public MetaWidgetAsyncDataInitializer(
			IMetaPrefabsContainer prefabsContainer, 
			IGuiEngine guiEngine, 
			IMetaMainWidgetModel metaMainWidgetModel) 
			: base(prefabsContainer, guiEngine)
			=> this.metaMainWidgetModel = metaMainWidgetModel;
		
		protected override void InitializeRegisteredWidget()
			=> widget.OnStartButtonClicked += OnStartButtonClicked;

		private void OnStartButtonClicked()
			=> metaMainWidgetModel.HandleStartButtonClick();
	}
}