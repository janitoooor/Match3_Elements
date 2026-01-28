namespace Base
{
	public interface IGameRegimeActivator : IProgressUpdatable
	{
		void ActivateRegime(float maxProgress, AsyncDataInitializerChainFinishedCallback finishedCallback);
	}
}