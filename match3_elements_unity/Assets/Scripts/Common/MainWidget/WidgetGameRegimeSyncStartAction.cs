using Base;

namespace Common.MainWidget
{
	public abstract class WidgetGameRegimeSyncStartAction<T> : GameRegimeSyncStartAction where T : IWidgetProvider
	{
		private readonly T widgetProvider;

		protected WidgetGameRegimeSyncStartAction(T widgetProvider)
			=> this.widgetProvider = widgetProvider;

		public override void Perform()
			=> widgetProvider.ShowWidget();
	}
}