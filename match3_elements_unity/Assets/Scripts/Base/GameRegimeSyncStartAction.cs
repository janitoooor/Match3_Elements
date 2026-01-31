using Zenject;

namespace Base
{
	public abstract class GameRegimeSyncStartAction : IGameRegimeSyncStartAction
	{
		public abstract byte priority { get; }
		
		[Inject]
		public void RegisterGameRegimeSyncStartAction(IGameRegimeSyncStartActionChainFiller chainFiller)
			=> chainFiller.AddGameRegimeSyncStartAction(this);
		
		public abstract void Perform();
	}
}