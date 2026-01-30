using Core.Blocks;
using UnityEngine;

namespace Core.Grid
{
	public delegate void MovedBlockDelegate(IBlockEntity blockEntity);
	
	public interface IGridField
	{
		void UpdateGridSize(int x, int y);
		void PlaceBlockAtCell(IBlockEntity blockEntity, Vector2Int cell);
		void MoveBlockToCell(IBlockEntity blockEntity, Vector2Int cell, MovedBlockDelegate callback);
	}
}