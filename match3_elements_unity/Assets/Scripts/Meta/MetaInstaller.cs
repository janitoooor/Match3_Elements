using Meta.MainWidget;
using UnityEngine;
using Zenject;

namespace Meta
{
	public sealed class MetaInstaller : MonoInstaller
	{
		[SerializeField]
		private MetaPrefabsContainer metaPrefabsContainer;

		public override void InstallBindings()
		{
			BindBaseMetaEntities();

			BindMainWidget();
		}

		private void BindBaseMetaEntities()
			=> Container.Bind<IMetaPrefabsContainer>().FromInstance(metaPrefabsContainer).AsSingle();

		private void BindMainWidget()
		{
			Container.Bind<MetaMainWidgetGameRegimeSyncStartAction>().AsSingle().NonLazy();
			
			Container.BindInterfacesTo<MetaWidgetAsyncDataInitializer>().AsSingle();

			Container.Bind<IMetaMainWidgetModel>().To<MetaMainWidgetModel>().AsSingle();
		}
	}
}