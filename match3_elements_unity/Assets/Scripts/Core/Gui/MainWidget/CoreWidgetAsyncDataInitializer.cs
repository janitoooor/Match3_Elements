using Base.Gui;
using Common.MainWidget;
using Core.Enums;
using Core.Interfaces;
using Zenject;

namespace Core.Gui.MainWidget
{
	public sealed class CoreWidgetAsyncDataInitializer : 
		WidgetAsyncDataInitialize<ICorePrefabsContainer, CorePrefabsKeys, CoreMainWidget, ICoreMainWidgetModel>, 
		ICoreWidgetProvider
	{
		protected override CorePrefabsKeys widgetKey => CorePrefabsKeys.MainWidget;

		public override byte priority => (byte)CoreAsyncDataInitializePriority.MainWidget;
		
		[Inject]
		public CoreWidgetAsyncDataInitializer(
			ICorePrefabsContainer prefabsContainer, 
			ICoreMainWidgetModel widgetModel, 
			IGuiEngine guiEngine) 
			: base(prefabsContainer, widgetModel, guiEngine) {}

		public void HideButtons()
			=> widget.HideButtons();
	}
}