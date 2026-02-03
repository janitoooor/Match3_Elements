using Base.Gui;
using Base.Gui.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Gui.MainWidget
{
	public sealed class CoreMainWidget : GuiWidget, ICoreMainWidget
	{
		[SerializeField]
		private Button restartButton;
		
		[SerializeField]
		private Button nextLevelButton;
		
		[SerializeField]
		private Button mainMenuButton;

		private void Awake()
		{
			AddButtonClickListener(restartButton, WidgetButtonType.Restart);
			AddButtonClickListener(nextLevelButton, WidgetButtonType.NextLevel);
			AddButtonClickListener(mainMenuButton, WidgetButtonType.MainMenu);
		}

		public void HideButtons()
		{
			restartButton.gameObject.SetActive(false);
			nextLevelButton.gameObject.SetActive(false);
			mainMenuButton.gameObject.SetActive(false);
		}
	}
}