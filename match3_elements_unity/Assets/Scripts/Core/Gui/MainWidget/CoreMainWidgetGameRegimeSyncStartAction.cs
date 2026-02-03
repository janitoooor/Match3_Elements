using Common.MainWidget;
using Core.Enums;
using Zenject;

namespace Core.Gui.MainWidget
{
	public sealed class CoreMainWidgetGameRegimeSyncStartAction : WidgetGameRegimeSyncStartAction<ICoreWidgetProvider>
	{
		public override byte priority => (byte)CoreGameRegimeSyncStartActionPriority.MainWidget;

		[Inject]
		public CoreMainWidgetGameRegimeSyncStartAction(ICoreWidgetProvider widgetProvider) : base(widgetProvider) {}
	}
}