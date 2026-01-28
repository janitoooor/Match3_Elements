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
			Container.BindInterfacesTo<MetaMainWidgetAsyncDataInitializer>().AsSingle();
			
			Container.Bind<MetaMainWidgetGameRegimeSyncStartAction>().AsSingle().NonLazy();

			Container.Bind<IMetaPrefabsContainer>().FromInstance(metaPrefabsContainer).AsSingle();
		}
	}
}