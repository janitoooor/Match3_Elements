using Base.Gui.Enums;

namespace Common.MainWidget
{
	public interface IWidgetModel
	{
		void HandleButtonClick(WidgetButtonType buttonType);
	}
}