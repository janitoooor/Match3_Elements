using System;
using System.Collections;
using Base;
using Base.Gui;
using Base.Gui.Enums;

namespace Common.MainWidget
{
	public abstract class WidgetAsyncDataInitialize<T, TP, TG, TC> : AsyncDataInitializer, IWidgetProvider, IDisposable 
		where T : IPrefabsContainer<TP>
		where TP : Enum
		where TG : GuiWidget
		where TC : IWidgetModel
	{
		private readonly T prefabsContainer;
		private readonly TC widgetModel;
		private readonly IGuiEngine guiEngine;
		
		protected TG widget;
		
		protected abstract TP widgetKey { get; }

		protected WidgetAsyncDataInitialize(
			T prefabsContainer,
			TC widgetModel,
			IGuiEngine guiEngine)
		{
			this.widgetModel = widgetModel;
			this.prefabsContainer = prefabsContainer;
			this.guiEngine = guiEngine;
		}

		public override IEnumerator Initialize()
		{
			var widgetPrefab = prefabsContainer.GetPrefab<TG>(widgetKey);
			widget = guiEngine.RegisterWidget(widgetPrefab);

			widget.OnWidgetButtonClicked += HandleButtonClick;
			
			AdditionalInitializeRegisteredWidget();
			
			yield return null;
		}

		private void HandleButtonClick(WidgetButtonType buttonType)
			=> widgetModel.HandleButtonClick(buttonType);

		protected virtual void AdditionalInitializeRegisteredWidget() {}

		public void ShowWidget()
			=> widget.Open();

		public void Dispose()
			=> widget?.Dispose();
	}
}