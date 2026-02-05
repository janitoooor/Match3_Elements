using Core.Gui.LevelWinWidget;
using Core.Gui.MainWidget;
using Zenject;

namespace Core.Gui
{
	public sealed class CoreGuiInstaller : Installer<CoreGuiInstaller>
	{
		public override void InstallBindings()
		{
			BindMainWidget();

			BindLevelWinWidget();
		}
		
		private void BindMainWidget()
		{
			Container.Bind<CoreMainWidgetGameRegimeSyncStartAction>().AsSingle().NonLazy();
			
			Container.BindInterfacesTo<CoreWidgetAsyncDataInitializer>().AsSingle();

			Container.Bind<ICoreMainWidgetModel>().To<CoreMainWidgetModel>().AsSingle();
		}
		
		private void BindLevelWinWidget()
		{
			Container.BindInterfacesTo<LevelWinWidgetAsyncDataInitializer>().AsSingle();

			Container.Bind<ILevelWinWidgetModel>().To<LevelWinWidgetModel>().AsSingle();
			
			Container.Bind<ILevelWinWidgetLevelRestarter>().To<LevelWinWidgetLevelRestarter>().AsSingle();
		}
	}
}