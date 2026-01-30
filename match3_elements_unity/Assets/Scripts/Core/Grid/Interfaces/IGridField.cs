using Core.Blocks;
using UnityEngine;

namespace Core.Grid
{
	public interface IGridField
	{
		void PrepareGridSize(int x, int y);
		void PlaceBlockAtCell(IBlockEntity blockEntity, Vector2Int cell);
	}
}