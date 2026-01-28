using System.Collections.Generic;
using System.Linq;

namespace Base
{
	public sealed class GameRegimeSyncStartActionChain : IGameRegimeSyncStartActionChain, IGameRegimeSyncStartActionChainFiller
	{
		private readonly List<IGameRegimeSyncStartAction> gameRegimeStartActions = new (0);
		
		public void AddGameRegimeSyncStartAction(IGameRegimeSyncStartAction regimeStartAction) 
			=> gameRegimeStartActions.Add(regimeStartAction);
			
		public void Initialize()
		{
			foreach (var regimeStartAction in GetOrderedGameRegimes())
				regimeStartAction.Perform();
			
			gameRegimeStartActions.Clear();
		}

		private IOrderedEnumerable<IGameRegimeSyncStartAction> GetOrderedGameRegimes()
			=> gameRegimeStartActions.OrderByDescending(a => a.priority);
	}
}