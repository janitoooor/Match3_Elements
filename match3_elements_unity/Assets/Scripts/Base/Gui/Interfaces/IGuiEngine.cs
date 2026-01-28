using UnityEngine;

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
		T RegisterWidget<T>(T widgetPrefab) where T : Object, IGuiWidget;
	}
}