using Core.Blocks;
using Core.Input;
using UnityEngine;

namespace Core.Grid
{
	public interface IBlocksOnGridFieldProvider
	{
		void AddBlockOnGrid(IBlockEntity blockEntity, Vector2Int cellPos);
		void ChangeGridSize(int x, int y);
		Vector2Int GetBlockCellToSwipe(IBlockEntity blockEntity, SwipeDirectionData swipeDirectionData);
		void SwipeBlockTo(IBlockEntity blockEntity, Vector2Int cellToSwap, SwipeDirectionData swipeDirectionData);
	}
}