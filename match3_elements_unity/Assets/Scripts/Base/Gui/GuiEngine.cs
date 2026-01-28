using UnityEngine;
using Object = UnityEngine.Object;

namespace Base.Gui
{
	/// <summary>
	/// Provides a common interface through which the game entities may access core game gui features.
	/// </summary>
	public sealed class GuiEngine : MonoBehaviour, IGuiEngine
	{
		[SerializeField]
		private ProgressLoadingView progressLoadingView;
		
		[SerializeField]
		private RectTransform widgetsParent;
		
		public T RegisterWidget<T>(T widgetPrefab) where T : Object, IGuiWidget
		{
			var createdWidget = Instantiate(widgetPrefab, widgetsParent);
			createdWidget.Close();
			return createdWidget;
		}
		
		public void ShowProgressLoadingView()
			=> progressLoadingView.Show();

		public void UpdateProgressLoadingView(float progress)
			=> progressLoadingView.IncreaseProgress(progress);

		public void HideProgressLoadingView()
			=> progressLoadingView.Hide();
	}
}