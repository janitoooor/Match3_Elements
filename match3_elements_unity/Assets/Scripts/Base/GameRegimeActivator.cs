using Zenject;

namespace Base
{
	public sealed class GameRegimeActivator : IGameRegimeActivator
	{
		public event ProgressUpdatedDelegate OnProgressUpdated
		{
			add => asyncDataInitializerChain.OnProgressUpdated += value;
			remove => asyncDataInitializerChain.OnProgressUpdated -= value;
		}
		
		private readonly IGameRegimeSyncStartActionChain gameRegimeSyncStartActionChain; 
		private readonly IAsyncDataInitializerChain asyncDataInitializerChain;

		[Inject]
		public GameRegimeActivator(
			IGameRegimeSyncStartActionChain gameRegimeSyncStartActionChain, 
			IAsyncDataInitializerChain asyncDataInitializerChain) 
		{
			this.gameRegimeSyncStartActionChain = gameRegimeSyncStartActionChain;
			this.asyncDataInitializerChain = asyncDataInitializerChain;
		}

		public void ActivateRegime(float maxProgress, AsyncDataInitializerChainFinishedCallback finishedCallback)
			=> asyncDataInitializerChain.Initialize(
				maxProgress, 
				()=> gameRegimeSyncStartActionChain.Initialize(()=> finishedCallback?.Invoke()));
	}
}