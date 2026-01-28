using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace Base
{
	public sealed class AsyncDataInitializerChain : IAsyncDataInitializerChain, IAsyncDataInitializerChainFiller
	{
		public event OnProgressUpdatedDelegate OnProgressUpdated;
		
		private List<IAsyncDataInitializer> dataInitializers = new (0);
		private readonly IAsyncProcessor asyncProcessor;
		
		[Inject]
		public AsyncDataInitializerChain(IAsyncProcessor asyncProcessor)
			=> this.asyncProcessor = asyncProcessor;

		public void AddAsyncDataInitializer(IAsyncDataInitializer dataInitializer) 
			=> dataInitializers.Add(dataInitializer);
		
		public void Initialize(float maxProgress, AsyncDataInitializerChainFinishedCallback finishedCallback)
			=> asyncProcessor.StartCoroutine(InitializersChainRoutine(maxProgress, finishedCallback));

		private IEnumerator InitializersChainRoutine(float maxProgress, 
			AsyncDataInitializerChainFinishedCallback finishedCallback)
		{
			yield return null;
			
			dataInitializers = dataInitializers.OrderByDescending(i => i.priority).ToList();
			
			var deltaProgress = maxProgress / dataInitializers.Count;
			
			foreach (var dataInitializer in dataInitializers)
				yield return Initialize(dataInitializer, deltaProgress);
			
			dataInitializers.Clear();
			
			finishedCallback?.Invoke();
		}

		private IEnumerator Initialize(IAsyncDataInitializer dataInitializer, float deltaProgress)
		{
			yield return dataInitializer.Initialize();
			OnProgressUpdated?.Invoke(deltaProgress);
		}
	}
}