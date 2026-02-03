using System;
using Base.Gui.Enums;

namespace Base.Gui
{
	public delegate void WidgetButtonClickedDelegate(WidgetButtonType buttonType);
	
	public interface IGuiWidget : IDisposable
	{
		event WidgetButtonClickedDelegate OnWidgetButtonClicked;
		
		void Open();
		void Close();
	}
}