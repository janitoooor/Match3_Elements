using Core.CoreCamera;
using Core.Input;
using Core.Interfaces;
using Core.SpawnContainer;
using UnityEngine;
using Zenject;

namespace Core
{
	public sealed class CoreInstaller : MonoInstaller
	{
		[SerializeField]
		private CorePrefabsContainer corePrefabsContainer;
		
		[SerializeField]
		private SpawnerInContainer spawnerInContainer;

		public override void InstallBindings()
		{
			Container.BindInterfacesTo<SwipeInputController>().AsSingle();
			
#if UNITY_EDITOR
			Container.Bind<ITickable>().To<CoreCameraGridFieldFitGameRegimeSyncStartAction>().AsSingle();
#else
			Container.Bind<CoreCameraGridFieldFitGameRegimeSyncStartAction>().AsSingle().NonLazy();
#endif
			
			Container.Bind<ISpawnerInContainer>().FromInstance(spawnerInContainer).AsSingle();
			
			Container.Bind<ICorePrefabsContainer>().FromInstance(corePrefabsContainer).AsSingle();
		}
	}
}