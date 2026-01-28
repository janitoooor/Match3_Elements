using Base.Gui;

namespace Meta.MainWidget
{
	public delegate void StartButtonClickedDelegate();
	
	public interface IMainWidget : IGuiWidget
	{
		event StartButtonClickedDelegate OnStartButtonClicked;
	}
}