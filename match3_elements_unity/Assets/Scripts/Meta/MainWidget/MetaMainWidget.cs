using Base.Gui;
using Base.Gui.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Meta.MainWidget
{
	public sealed class MetaMainWidget : GuiWidget, IMetaMainWidget
	{
		[SerializeField]
		private Button startButton;
		
		[SerializeField]
		private Button clearSavesButton;

		private void Awake()
		{
			AddButtonClickListener(startButton, WidgetButtonType.StartGame);
			AddButtonClickListener(clearSavesButton, WidgetButtonType.ClearSaves);
		}
	}
}