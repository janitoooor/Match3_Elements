using Base.Gui;
using Base.Gui.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Gui.LevelWinWidget
{
	public sealed class LevelWinWidget : GuiWidget, ILevelWinWidget
	{
		[SerializeField]
		private Button restartButton;
		
		[SerializeField]
		private Button nextLevelButton;
		
		[SerializeField]
		private Button mainButton;

		private void Awake()
		{
			AddButtonClickListener(restartButton, WidgetButtonType.Restart);
			AddButtonClickListener(nextLevelButton, WidgetButtonType.NextLevel);
			AddButtonClickListener(mainButton, WidgetButtonType.MainMenu);
		}
	}
}