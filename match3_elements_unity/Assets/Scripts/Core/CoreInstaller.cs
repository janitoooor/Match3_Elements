using Core.Interfaces;
using Core.MainWidget;
using UnityEngine;
using Zenject;

namespace Core
{
	public sealed class CoreInstaller : MonoInstaller
	{
		[SerializeField]
		private CorePrefabsContainer corePrefabsContainer;

		public override void InstallBindings()
		{
			BindBaseMetaEntities();

			BindMainWidget();
		}

		private void BindBaseMetaEntities()
			=> Container.Bind<ICorePrefabsContainer>().FromInstance(corePrefabsContainer).AsSingle();

		private void BindMainWidget()
		{
			Container.Bind<CoreMainWidgetGameRegimeSyncStartAction>().AsSingle().NonLazy();
			
			Container.BindInterfacesTo<CoreWidgetAsyncDataInitializer>().AsSingle();

			Container.Bind<ICoreMainWidgetModel>().To<CoreMainWidgetModel>().AsSingle();
		}
	}
}