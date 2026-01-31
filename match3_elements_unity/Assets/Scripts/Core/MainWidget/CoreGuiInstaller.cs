using Zenject;

namespace Core.MainWidget
{
	public sealed class CoreGuiInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			BindMainWidget();
		}
		
		private void BindMainWidget()
		{
			Container.Bind<CoreMainWidgetGameRegimeSyncStartAction>().AsSingle().NonLazy();
			
			Container.BindInterfacesTo<CoreWidgetAsyncDataInitializer>().AsSingle();

			Container.Bind<ICoreMainWidgetModel>().To<CoreMainWidgetModel>().AsSingle();
		}
	}
}