using Core.Blocks;
using UnityEngine;

namespace Core.Grid
{
	public interface IBlockOnGridRendererSortingOderProvider
	{
		void SetSourceBlockRendererSortingOderBeforeMove(IBlockEntity blockEntity, int finishSortingOrder,
			int targetCellY, int gridFieldSizeY);
		int CalculateSourceBlockRendererSortingOderBeforeMove(IBlockEntity blockEntity, int finishSortingOrder,
			int targetCellY, int gridFieldSizeY);
		void SetBlockRendererSortingOderForCell(IBlockEntity blockEntity, Vector2Int cell);
		int CalculateBlockRendererSortingOrderForCell(int x, int y);
	}
}