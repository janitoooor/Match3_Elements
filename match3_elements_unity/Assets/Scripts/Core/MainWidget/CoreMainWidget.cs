using Base.Gui;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MainWidget
{
	public sealed class CoreMainWidget : GuiWidget, ICoreMainWidget
	{
		[SerializeField]
		private Button restartButton;
		
		[SerializeField]
		private Button nextLevelButton;
	}
}