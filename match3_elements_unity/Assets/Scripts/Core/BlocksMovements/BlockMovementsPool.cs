using System.Collections.Generic;
using Zenject;

namespace Core.BlocksMovements
{
	public sealed class BlockMovementsPool : IBlockMovementsPool
	{
		private readonly IBlockMovementFactory blockMovementFactory;
		
		private readonly Queue<IBlockMovement> cacheMovements;

		[Inject]
		public BlockMovementsPool(
			IBlockMovementFactory blockMovementFactory,
			IBlockMovementsPoolCacheSizeCalculator poolCacheSizeCalculator)
		{
			this.blockMovementFactory = blockMovementFactory;

			var cacheSize = poolCacheSizeCalculator.CalculateCacheSize();
			cacheMovements = new Queue<IBlockMovement>(cacheSize);

			for (var i = 0; i < cacheSize; i++)
				cacheMovements.Enqueue(blockMovementFactory.Create());
		}
		
		public IBlockMovement GetBlockMovement()
			=> cacheMovements.Count > 0 
				? cacheMovements.Dequeue() 
				: blockMovementFactory.Create();

		public void AddBlockMovement(IBlockMovement blockMovement)
			=> cacheMovements.Enqueue(blockMovement);
	}
}