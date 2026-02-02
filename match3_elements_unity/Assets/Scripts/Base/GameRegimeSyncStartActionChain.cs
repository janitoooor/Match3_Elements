using System.Collections.Generic;
using System.Linq;

namespace Base
{
	public sealed class GameRegimeSyncStartActionChain : IGameRegimeSyncStartActionChain, IGameRegimeSyncStartActionChainFiller
	{
		private readonly List<IGameRegimeSyncStartAction> gameRegimeStartActions = new (0);
		
		public void AddGameRegimeSyncStartAction(IGameRegimeSyncStartAction regimeStartAction) 
			=> gameRegimeStartActions.Add(regimeStartAction);
			
		public void Initialize(GameRegimeSyncStartActionChainFinishedDelegate finishedCallback)
		{
			foreach (var regimeStartAction in GetOrderedGameRegimes())
				regimeStartAction.Perform();
			
			gameRegimeStartActions.Clear();
			finishedCallback?.Invoke();
		}

		private IOrderedEnumerable<IGameRegimeSyncStartAction> GetOrderedGameRegimes()
			=> gameRegimeStartActions.OrderBy(a => a.priority);
	}
}