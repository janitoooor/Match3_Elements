using Core.Blocks;
using Core.Input;
using UnityEngine;

namespace Core.BlocksSwipe
{
	public delegate void KillBlocksInLineRequestDelegate(IBlockEntity block, Vector2Int blockCell);
	
	public interface IBlocksOnGridFieldMover
	{
		event KillBlocksInLineRequestDelegate OnKillBlocksInLineRequest;
		void SwipeBlockTo(IBlockEntity blockEntity, Vector2Int cellToSwap, SwipeDirectionData swipeDirectionData);
		void TryFallUpNeighboursAfterBlockMoved(Vector2Int blockCell);
		bool IsBlockUnAvailable(IBlockEntity block);
	}
}