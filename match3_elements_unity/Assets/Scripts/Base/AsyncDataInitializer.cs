using System.Collections;
using Zenject;

namespace Base
{
	public abstract class AsyncDataInitializer : IAsyncDataInitializer
	{
		public abstract int priority { get; }
		
		[Inject]
		public void RegisterAsyncLoadedData(IAsyncDataInitializerChainFiller dataInitializerChainFiller)
			=> dataInitializerChainFiller.AddAsyncDataInitializer(this);
		
		public abstract IEnumerator Initialize();
	}
}