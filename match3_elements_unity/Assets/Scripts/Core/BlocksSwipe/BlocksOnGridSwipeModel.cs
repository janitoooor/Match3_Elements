using Core.Blocks;
using Core.Grid;
using Core.Input;
using UnityEngine;
using Zenject;

namespace Core.BlocksSwipe
{
	public sealed class BlocksOnGridSwipeModel : IBlocksOnGridSwipeModel
	{
		private readonly IBlocksOnGridRepository blocksOnGridRepository;
		private readonly IBlocksOnGridFieldMover blocksOnGridFieldMover;

		[Inject]
		public BlocksOnGridSwipeModel(
			IBlocksOnGridRepository blocksOnGridRepository, 
			IBlocksOnGridFieldMover blocksOnGridFieldMover)
		{
			this.blocksOnGridRepository = blocksOnGridRepository;
			this.blocksOnGridFieldMover = blocksOnGridFieldMover;
		}

		public void TrySwipeBlockTo(IBlockEntity blockEntity, SwipeDirectionData swipeDirectionData)
		{
			var cellToSwap = blocksOnGridRepository.GetBlockCellToSwipe(blockEntity, swipeDirectionData);
			
			if (IsCellToSwapInsideGridField(cellToSwap, blocksOnGridRepository.gridSize))
				blocksOnGridFieldMover.SwipeBlockTo(blockEntity, cellToSwap, swipeDirectionData);
		}

		private static bool IsCellToSwapInsideGridField(Vector2Int blockCellAfterSwipe, Vector2Int gridSize)
			=> blockCellAfterSwipe.x >= 0
			   && blockCellAfterSwipe.y >= 0
			   && blockCellAfterSwipe.x < gridSize.x 
			   && blockCellAfterSwipe.y < gridSize.y;
	}
}