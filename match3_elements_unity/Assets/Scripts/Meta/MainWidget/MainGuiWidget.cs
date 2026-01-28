using Base.Gui;
using UnityEngine;
using UnityEngine.UI;

namespace Meta.MainWidget
{
	public sealed class MainGuiWidget : GuiWidget, IMainWidget
	{
		public event StartButtonClickedDelegate OnStartButtonClicked;
		
		[SerializeField]
		private Button startButton;

		private void Awake()
			=> startButton.onClick.AddListener(StartButtonClick);

		private void StartButtonClick()
			=> OnStartButtonClicked?.Invoke();
	}
}