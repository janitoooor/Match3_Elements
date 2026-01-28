using Base;
using Meta.Enums;
using Zenject;

namespace Meta.MainWidget
{
	public sealed class MetaMainWidgetGameRegimeSyncStartAction : GameRegimeSyncStartAction
	{
		private readonly IMainWidgetProvider mainWidgetProvider;
		public override int priority => (int)MetaGameRegimeSyncStartActionPriority.MainWidget;

		[Inject]
		public MetaMainWidgetGameRegimeSyncStartAction(IMainWidgetProvider mainWidgetProvider)
			=> this.mainWidgetProvider = mainWidgetProvider;

		public override void Perform()
			=> mainWidgetProvider.ShowMetaWidget();
	}
}