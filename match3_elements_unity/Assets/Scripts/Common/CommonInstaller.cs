using Common.Saves;
using Common.Saves.Level;
using Zenject;

namespace Common
{
	public sealed class CommonInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.Bind<ISavesWriter>().To<PlayerPrefsSavesWriter>().AsSingle();
			
			Container.Bind<ICurrentLevelSavesStorage>().To<CurrentLevelSavesStorage>().AsSingle();
		}
	}
}