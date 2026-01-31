using Core.Level;
using UnityEngine;
using Zenject;

namespace Core.BlocksMovements
{
	public sealed class BlockMovementsPoolCacheSizeCalculator : IBlockMovementsPoolCacheSizeCalculator
	{
		private readonly ICurrentLevelDataProvider currentLevelDataProvider;

		[Inject]
		public BlockMovementsPoolCacheSizeCalculator(ICurrentLevelDataProvider currentLevelDataProvider)
			=> this.currentLevelDataProvider = currentLevelDataProvider;

		public int CalculateCacheSize()
		{
			var currentLevelData = currentLevelDataProvider.GetCurrentLevelData();
			var levelGridSize = currentLevelData.gridSize;
			return levelGridSize.x * levelGridSize.y / Mathf.Max(currentLevelData.minDestroyBlocksLineLength, 2);
		}
	}
}