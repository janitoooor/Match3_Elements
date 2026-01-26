using UnityEngine;

namespace Base.Gui
{
	/// <summary>
	/// Provides a common interface through which the game entities may access core game gui features.
	/// </summary>
	public sealed class GuiEngine : MonoBehaviour, IGuiEngine
	{
		[SerializeField]
		private ProgressLoadingView progressLoadingView;
		
		public void ShowProgressLoadingView()
			=> progressLoadingView.Show();

		public void UpdateProgressLoadingView(float progress)
			=> progressLoadingView.UpdateProgress(progress);

		public void HideProgressLoadingView()
			=> progressLoadingView.Hide();
	}
}