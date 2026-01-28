using Zenject;

namespace Base
{
	public sealed class GameRegimeActivator : IGameRegimeActivator
	{
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
			=> asyncDataInitializerChain.Initialize(maxProgress, InitializedCallback(finishedCallback));

		private AsyncDataInitializerChainFinishedCallback InitializedCallback(
			AsyncDataInitializerChainFinishedCallback finishedCallback)
			=> () =>
			{
				gameRegimeSyncStartActionChain.Initialize();
				finishedCallback?.Invoke();
			};
	}
}