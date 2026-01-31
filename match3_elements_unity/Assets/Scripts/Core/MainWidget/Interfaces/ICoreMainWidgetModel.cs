using Core.Enums;

namespace Core.MainWidget
{
	public interface ICoreMainWidgetModel
	{
		void HandleButtonClick(CoreMainWidgetButtonType buttonType);
	}
}