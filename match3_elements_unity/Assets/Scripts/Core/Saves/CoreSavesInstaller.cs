using Zenject;

namespace Core.Saves
{
	public sealed class CoreSavesInstaller : Installer<CoreSavesInstaller>
	{
		public override void InstallBindings()
		{
			Container.Bind<IBlocksOnGridCoreSavesStorage>().To<BlocksOnGridCoreSavesStorage>().AsSingle().NonLazy();
		}
	}
}