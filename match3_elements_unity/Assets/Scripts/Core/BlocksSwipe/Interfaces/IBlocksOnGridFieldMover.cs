using Core.Blocks;
using Core.Input;
using UnityEngine;

namespace Core.BlocksSwipe
{
	public interface IBlocksOnGridFieldMover
	{
		void SwipeBlockTo(IBlockEntity blockEntity, Vector2Int cellToSwap, SwipeDirectionData swipeDirectionData);
		void TryFallUpNeighboursAfterBlockMoved(Vector2Int blockCell);
		void TryFallBlockFrom(IBlockEntity blockEntity, Vector2Int blockCell);
		bool IsBlockUnAvailable(IBlockEntity block);
	}
}