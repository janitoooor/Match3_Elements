using Base.Gui;

namespace Meta.MainWidget
{
	public delegate void MetaStartButtonClickedDelegate();
	
	public interface IMetaMainWidget : IGuiWidget
	{
		event MetaStartButtonClickedDelegate OnStartButtonClicked;
	}
}