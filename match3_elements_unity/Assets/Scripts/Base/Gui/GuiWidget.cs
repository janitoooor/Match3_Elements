using UnityEngine;

namespace Base.Gui
{
	public abstract class GuiWidget : MonoBehaviour, IGuiWidget
	{
		public void Open()
		{
			gameObject.SetActive(true);
			transform.SetAsLastSibling();
		}

		public void Close()
			=> gameObject.SetActive(false);

		public void Dispose()
			=> Destroy(gameObject);
	}
}