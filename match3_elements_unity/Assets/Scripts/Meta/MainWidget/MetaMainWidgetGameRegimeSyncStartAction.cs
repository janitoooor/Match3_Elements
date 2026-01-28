using Common.MainWidget;
using Meta.Enums;
using Zenject;

namespace Meta.MainWidget
{
	public sealed class MetaMainWidgetGameRegimeSyncStartAction : WidgetGameRegimeSyncStartAction<IMetaWidgetProvider>
	{
		public override int priority => (int)MetaGameRegimeSyncStartActionPriority.MainWidget;

		[Inject]
		public MetaMainWidgetGameRegimeSyncStartAction(IMetaWidgetProvider widgetProvider) : base(widgetProvider) {}
	}
}