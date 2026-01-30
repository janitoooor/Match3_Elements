using UnityEngine;
using UnityEngine.UI;

namespace Base.Gui
{
	public sealed class ProgressLoadingView : MonoBehaviour
	{
		[SerializeField]
		private Image progressBar;
		
		[SerializeField]
		private Image maxBar;

		private float maxWidth;
		
		private float currentProgress;

		private void Start()
			=> UpdateProgressView();

		public void IncreaseProgress(float progressDelta)
			=> UpdateProgress(currentProgress + progressDelta);

		private void UpdateProgressView()
		{
			maxWidth = maxBar.rectTransform.rect.width;
			progressBar.rectTransform.sizeDelta = CalculateRectTransformSizeDelta();
		}

		private Vector2 CalculateRectTransformSizeDelta()
			=> new(currentProgress * maxWidth, progressBar.rectTransform.sizeDelta.y);

		public void Hide()
			=> gameObject.SetActive(false);

		public void Show()
		{
			UpdateProgress(0f);

			gameObject.SetActive(true);
		}

		private void UpdateProgress(float newProgress)
		{
			currentProgress = Mathf.Clamp01(newProgress);
			UpdateProgressView();
		}
	}
}