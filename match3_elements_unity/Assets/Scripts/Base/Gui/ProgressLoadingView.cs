using UnityEngine;
using UnityEngine.UI;

namespace Base.Gui
{
	public sealed class ProgressLoadingView : MonoBehaviour
	{
		[SerializeField]
		private Image progressBar;
		
		public void UpdateProgress(float progress)
			=> progressBar.fillAmount = progress;

		public void Hide()
			=> gameObject.SetActive(false);

		public void Show()
			=> gameObject.SetActive(true);
	}
}