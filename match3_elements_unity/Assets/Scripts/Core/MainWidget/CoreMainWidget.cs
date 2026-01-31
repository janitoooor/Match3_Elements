using Base.Gui;
using Core.Enums;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core.MainWidget
{
	public sealed class CoreMainWidget : GuiWidget, ICoreMainWidget
	{
		public event CoreMainWidgetButtonClickedDelegate OnCoreMainWidgetButtonClicked;
		
		[SerializeField]
		private Button restartButton;
		
		[SerializeField]
		private Button nextLevelButton;

		private void Awake()
		{
			restartButton.onClick.AddListener(ButtonClickedCall(CoreMainWidgetButtonType.Restart));
			nextLevelButton.onClick.AddListener(ButtonClickedCall(CoreMainWidgetButtonType.NextLevel));
		}

		private UnityAction ButtonClickedCall(CoreMainWidgetButtonType buttonType)
			=> ()=> OnCoreMainWidgetButtonClicked?.Invoke(buttonType);
	}
}