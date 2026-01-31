using Base.Gui;
using Core.Enums;

namespace Core.MainWidget
{
	public delegate void CoreMainWidgetButtonClickedDelegate(CoreMainWidgetButtonType buttonType);
	
	public interface ICoreMainWidget : IGuiWidget
	{
		event CoreMainWidgetButtonClickedDelegate OnCoreMainWidgetButtonClicked;
	}
}