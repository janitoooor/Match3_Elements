using Core.Blocks;
using Core.Input;
using UnityEngine;
using Zenject;

namespace Core.Grid
{
	public sealed class BlocksOnGridSwipeModel : IBlocksOnGridSwipeModel
	{
		private readonly IGridFieldInfo gridFieldInfo;
		private readonly IBlocksOnGridFieldProvider blocksOnGridFieldProvider;

		[Inject]
		public BlocksOnGridSwipeModel(
			IGridFieldInfo gridFieldInfo, 
			IBlocksOnGridFieldProvider blocksOnGridFieldProvider)
		{
			this.gridFieldInfo = gridFieldInfo;
			this.blocksOnGridFieldProvider = blocksOnGridFieldProvider;
		}

		public void TrySwipeBlockTo(IBlockEntity blockEntity, SwipeDirectionData swipeDirectionData)
		{
			var cellToSwap = blocksOnGridFieldProvider.GetBlockCellToSwipe(blockEntity, swipeDirectionData);
			
			if (IsCellToSwapInsideGridField(cellToSwap, gridFieldInfo.gridSize))
				blocksOnGridFieldProvider.SwipeBlockTo(blockEntity, cellToSwap, swipeDirectionData);
		}

		private static bool IsCellToSwapInsideGridField(Vector2Int blockCellAfterSwipe, Vector2Int gridSize)
			=> blockCellAfterSwipe.x >= 0
			   && blockCellAfterSwipe.y >= 0
			   && blockCellAfterSwipe.x < gridSize.x 
			   && blockCellAfterSwipe.y < gridSize.y;
	}
}