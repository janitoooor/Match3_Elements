using Base.Gui;
using Common.MainWidget;
using Core.Enums;
using Core.Interfaces;
using Zenject;

namespace Core.Gui.LevelWinWidget
{
	public sealed class LevelWinWidgetAsyncDataInitializer : 
		WidgetAsyncDataInitialize<ICorePrefabsContainer, CorePrefabsKeys, LevelWinWidget, ILevelWinWidgetModel>, 
		ILevelWinWidgetProvider
	{
		protected override CorePrefabsKeys widgetKey => CorePrefabsKeys.LevelWinWidget;

		public override byte priority => (byte)CoreAsyncDataInitializePriority.LevelWinWidget;
		
		[Inject]
		public LevelWinWidgetAsyncDataInitializer(
			ICorePrefabsContainer prefabsContainer, 
			ILevelWinWidgetModel widgetModel, 
			IGuiEngine guiEngine) 
			: base(prefabsContainer, widgetModel, guiEngine) {}
	}
}