namespace Base
{
	public delegate void AsyncDataInitializerChainFinishedCallback();
	
	public interface IAsyncDataInitializerChain : IProgressUpdatable
	{
		void Initialize(float maxProgress, AsyncDataInitializerChainFinishedCallback finishedCallback);
	}
}