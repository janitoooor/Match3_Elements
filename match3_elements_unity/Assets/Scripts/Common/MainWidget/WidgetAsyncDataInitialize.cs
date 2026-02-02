using System;
using System.Collections;
using Base;
using Base.Gui;

namespace Common.MainWidget
{
	public abstract class WidgetAsyncDataInitialize<T, TP, TG> : AsyncDataInitializer, IWidgetProvider, IDisposable 
		where T : IPrefabsContainer<TP>
		where TP : Enum
		where TG : GuiWidget
	{
		private readonly T prefabsContainer;
		private readonly IGuiEngine guiEngine;
		
		protected TG widget;
		
		protected abstract TP widgetKey { get; }

		protected WidgetAsyncDataInitialize(
			T prefabsContainer,
			IGuiEngine guiEngine)
		{
			this.prefabsContainer = prefabsContainer;
			this.guiEngine = guiEngine;
		}

		public override IEnumerator Initialize()
		{
			var widgetPrefab = prefabsContainer.GetPrefab<TG>(widgetKey);
			widget = guiEngine.RegisterWidget(widgetPrefab);

			InitializeRegisteredWidget();
			
			yield return null;
		}

		protected abstract void InitializeRegisteredWidget();

		public void ShowWidget()
			=> widget.Open();

		public void Dispose()
			=> widget?.Dispose();
	}
}