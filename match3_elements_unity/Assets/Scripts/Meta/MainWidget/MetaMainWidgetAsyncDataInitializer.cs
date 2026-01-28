using System;
using System.Collections;
using Base;
using Base.Gui;
using Meta.Enums;
using Zenject;

namespace Meta.MainWidget
{
	public sealed class MetaMainWidgetAsyncDataInitializer : AsyncDataInitializer, IMainWidgetProvider, IDisposable
	{
		private readonly IMetaPrefabsContainer metaPrefabsContainer;
		private readonly IGuiEngine guiEngine;
		
		private IMainWidget mainWidget;

		public override int priority => (int)MetaAsyncDataInitializePriority.MainWidget;
		
		[Inject]
		public MetaMainWidgetAsyncDataInitializer(IMetaPrefabsContainer metaPrefabsContainer, IGuiEngine guiEngine)
		{
			this.metaPrefabsContainer = metaPrefabsContainer;
			this.guiEngine = guiEngine;
		}
		
		public override IEnumerator Initialize()
		{
			var widgetPrefab = metaPrefabsContainer.GetPrefab<MainGuiWidget>(MetaPrefabsKeys.MainWidget);
			mainWidget = guiEngine.RegisterWidget(widgetPrefab);
			yield return null;
		}

		public void ShowMetaWidget()
			=> mainWidget.Open();

		public void Dispose()
			=> mainWidget?.Dispose();
	}
}