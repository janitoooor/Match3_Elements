using System.Collections.Generic;
using Core.Blocks;
using Core.Input;
using UnityEngine;

namespace Core.Grid
{
	public interface IBlocksOnGridFieldProvider
	{
		IReadOnlyDictionary<IBlockEntity, Vector2Int> blocksOnGridField { get; }
		void AddBlockOnGrid(IBlockEntity blockEntity, Vector2Int cell);
		void SetGridSize(int x, int y);
		Vector2Int GetBlockCellToSwipe(IBlockEntity blockEntity, SwipeDirectionData swipeDirectionData);
		void SwipeBlockTo(IBlockEntity blockEntity, Vector2Int cellToSwap, SwipeDirectionData swipeDirectionData);
	}
}