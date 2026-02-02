using UnityEngine;
using Zenject;

namespace Core.Balloons
{
	public sealed class BalloonsInstaller : MonoInstaller
	{
		[SerializeField]
		private BalloonsContainer balloonsContainer;
		
		public override void InstallBindings()
		{
			Container.Bind<IBalloonsContainer>().FromInstance(balloonsContainer).AsSingle();
			
			Container.Bind<BalloonsAsyncDataInitializer>().AsSingle().NonLazy();
			
			Container.Bind<BalloonsGameRegimeSyncStartAction>().AsSingle().NonLazy();
			
			Container.Bind<IBalloonGenerator>().To<BalloonGenerator>().AsSingle();
			
			Container.BindInterfacesTo<BallonsFlyProcessor>().AsSingle();
		}
	}
}