using Common.MainWidget;
using Core.Enums;
using Zenject;

namespace Core.MainWidget
{
	public sealed class CoreMainWidgetGameRegimeSyncStartAction : WidgetGameRegimeSyncStartAction<ICoreWidgetProvider>
	{
		public override int priority => (int)CoreGameRegimeSyncStartActionPriority.MainWidget;

		[Inject]
		public CoreMainWidgetGameRegimeSyncStartAction(ICoreWidgetProvider widgetProvider) : base(widgetProvider) {}
	}
}