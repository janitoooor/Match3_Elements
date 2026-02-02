using Core.Blocks;
using UnityEngine;

namespace Core.Grid
{
	public sealed class BlocksOnGridRendererSortingOderProvider : IBlockOnGridRendererSortingOderProvider
	{
		private const int CELL_SORTING_ORDER = 100; 
		
		public void SetSourceBlockRendererSortingOderBeforeMove(IBlockEntity blockEntity, int finishSortingOrder, 
			int targetCellY, int gridFieldSizeY)
			=> blockEntity.SetRendererSortingOder(CalculateSourceBlockRendererSortingOderBeforeMove(
				blockEntity, 
				finishSortingOrder, 
				targetCellY,
				gridFieldSizeY));
		
		public int CalculateSourceBlockRendererSortingOderBeforeMove(IBlockEntity blockEntity, int finishSortingOrder,
			int targetCellY, int gridFieldSizeY)
			=> Mathf.Max(blockEntity.rendererSortingOrder, finishSortingOrder) - (gridFieldSizeY - targetCellY);

		public void SetBlockRendererSortingOderForCell(IBlockEntity blockEntity, Vector2Int cell)
			=> blockEntity.SetRendererSortingOder(CalculateBlockRendererSortingOrderForCell(cell.x, cell.y));
		
		public int CalculateBlockRendererSortingOrderForCell(int x, int y)
			=> CELL_SORTING_ORDER * (x + y);
	}
}