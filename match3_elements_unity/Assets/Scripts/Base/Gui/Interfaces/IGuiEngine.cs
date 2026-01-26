namespace Base.Gui
{
	/// <summary>
	/// Provides a common interface through which the game entities may access core game gui features.
	/// </summary>
	public interface IGuiEngine
	{
		void ShowProgressLoadingView();
		void UpdateProgressLoadingView(float progress);
		void HideProgressLoadingView();
	}
}