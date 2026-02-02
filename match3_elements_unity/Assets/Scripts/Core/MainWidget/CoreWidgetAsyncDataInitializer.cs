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

		protected override CorePrefabsKeys widgetKey => CorePrefabsKeys.MainWidget;

		public override byte priority => (byte)CoreAsyncDataInitializePriority.MainWidget;
		
		[Inject]
		public CoreWidgetAsyncDataInitializer(
			ICorePrefabsContainer prefabsContainer, 
			IGuiEngine guiEngine, 
			ICoreMainWidgetModel coreMainWidgetModel) 
			: base(prefabsContainer, guiEngine)
			=> this.coreMainWidgetModel = coreMainWidgetModel;
		
		protected override void InitializeRegisteredWidget()
			=> widget.OnCoreMainWidgetButtonClicked += OnCoreMainWidgetButtonClicked;

		private void OnCoreMainWidgetButtonClicked(CoreMainWidgetButtonType buttonType)
			=> coreMainWidgetModel.HandleButtonClick(buttonType);
	}
}