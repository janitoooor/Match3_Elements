namespace Base
{
	public interface IGameRegimeActivator
	{
		void ActivateRegime(float maxProgress, AsyncDataInitializerChainFinishedCallback finishedCallback);
	}
}