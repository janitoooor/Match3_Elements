using Base.Gui;
using Common.MainWidget;
using Meta.Enums;
using Zenject;

namespace Meta.MainWidget
{
	public sealed class MetaWidgetAsyncDataInitializer : WidgetAsyncDataInitialize<
		IMetaPrefabsContainer, 
		MetaPrefabsKeys,
		MetaMainWidget,
		IMetaMainWidgetModel>,
		IMetaWidgetProvider
	{
		protected override MetaPrefabsKeys widgetKey => MetaPrefabsKeys.MainWidget;

		public override byte priority => (byte)MetaAsyncDataInitializePriority.MainWidget;
		
		[Inject]
		public MetaWidgetAsyncDataInitializer(
			IMetaPrefabsContainer prefabsContainer, 
			IGuiEngine guiEngine, 
			IMetaMainWidgetModel widgetModel) 
			: base(prefabsContainer, widgetModel, guiEngine) {}
	}
}