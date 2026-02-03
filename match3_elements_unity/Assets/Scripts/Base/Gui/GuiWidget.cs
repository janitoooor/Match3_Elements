using Base.Gui.Enums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Base.Gui
{
	public abstract class GuiWidget : MonoBehaviour, IGuiWidget
	{
		public event WidgetButtonClickedDelegate OnWidgetButtonClicked;
		
		public void Open()
		{
			gameObject.SetActive(true);
			transform.SetAsLastSibling();
		}

		public void Close()
			=> gameObject.SetActive(false);

		public void Dispose()
			=> Destroy(gameObject);
		
		protected void AddButtonClickListener(Button button, WidgetButtonType widgetButtonType)
			=> button.onClick.AddListener(ButtonClickedCall(widgetButtonType));

		private UnityAction ButtonClickedCall(WidgetButtonType buttonType)
			=> ()=> OnWidgetButtonClicked?.Invoke(buttonType);
	}
}