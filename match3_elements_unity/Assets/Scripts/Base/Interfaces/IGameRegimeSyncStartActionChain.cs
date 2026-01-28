namespace Base
{
	public delegate void GameRegimeSyncStartActionChainFinishedDelegate();
	public interface IGameRegimeSyncStartActionChain
	{
		void Initialize(GameRegimeSyncStartActionChainFinishedDelegate finishedCallback);
	}
}