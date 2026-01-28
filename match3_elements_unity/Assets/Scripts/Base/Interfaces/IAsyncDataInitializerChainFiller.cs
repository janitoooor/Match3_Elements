namespace Base
{
	public interface IAsyncDataInitializerChainFiller : IProgressUpdatable
	{
		void AddAsyncDataInitializer(IAsyncDataInitializer dataInitializer);
	}
}