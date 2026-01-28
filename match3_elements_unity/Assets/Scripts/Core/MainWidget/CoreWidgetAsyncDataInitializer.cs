using Base.Gui;
using Common.MainWidget;
using Core.Enums;
using Core.Interfaces;
using Zenject;

namespace Core.MainWidget
{
	public sealed class CoreWidgetAsyncDataInitializer : 
		WidgetAsyncDataInitialize<ICorePrefabsContainer, CorePrefabsKeys, CoreMainWidget>, 
		ICoreWidgetProvider
	{
		private readonly ICoreMainWidgetModel coreMainWidgetModel;

		private protected override CorePrefabsKeys widgetKey => CorePrefabsKeys.MainWidget;

		public override int priority => (int)CoreAsyncDataInitializePriority.MainWidget;
		
		[Inject]
		public CoreWidgetAsyncDataInitializer(
			ICorePrefabsContainer prefabsContainer, 
			IGuiEngine guiEngine, 
			ICoreMainWidgetModel coreMainWidgetModel) 
			: base(prefabsContainer, guiEngine)
			=> this.coreMainWidgetModel = coreMainWidgetModel;
		
		private protected override void InitializeRegisteredWidget()
		{
		}
	}
}