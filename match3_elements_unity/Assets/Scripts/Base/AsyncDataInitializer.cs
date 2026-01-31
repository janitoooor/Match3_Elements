using System.Collections;
using Zenject;

namespace Base
{
	public abstract class AsyncDataInitializer : IAsyncDataInitializer
	{
		public abstract byte priority { get; }
		
		[Inject]
		public void RegisterAsyncLoadedData(IAsyncDataInitializerChainFiller dataInitializerChainFiller)
			=> dataInitializerChainFiller.AddAsyncDataInitializer(this);
		
		public abstract IEnumerator Initialize();
	}
}