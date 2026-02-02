using System.Collections.Generic;
using Core.Blocks;
using Core.Input;
using UnityEngine;
using Zenject;

namespace Core.Grid
{
	public sealed class BlocksOnGridRepository : IBlocksOnGridRepository
	{
		public event OnBlocksOnGridChangedDelegate OnBlockOnGridChanged;
		
		private readonly IGridField gridField;
		
		private readonly Dictionary<IBlockEntity, Vector2Int> blocksOnGridFieldInternal = new();
		private Dictionary<Vector2Int, IBlockEntity> gridCellsInternal;
		private readonly HashSet<Vector2Int> busyCells = new();
		private readonly HashSet<Vector2Int> busyCellsByKill = new();
		private readonly HashSet<IBlockEntity> killedBlocksInternal = new();

		public IReadOnlyCollection<IBlockEntity> killedBlocks => killedBlocksInternal;
		public IReadOnlyDictionary<IBlockEntity, Vector2Int> blocksOnGridField => blocksOnGridFieldInternal;
		public IReadOnlyDictionary<Vector2Int, IBlockEntity> gridCells => gridCellsInternal;
		public Vector2Int gridSize => gridField.gridSize;

		[Inject]
		public BlocksOnGridRepository(IGridField gridField)
			=> this.gridField = gridField;

		public void AddBlockOnGrid(IBlockEntity blockEntity, Vector2Int cell)
		{
			blockEntity.PlaceAt(
				gridField.GetTransformParentForBlocksInCell(), 
				CalculateBlockLocalPositionOnGridCell(cell));
			
			blocksOnGridFieldInternal.Add(blockEntity, cell);
			gridCellsInternal[cell] = blockEntity;
			
			OnBlockOnGridChanged?.Invoke(cell, blockEntity);
		}
		
		public void SetGridSize(int x, int y)
		{
			gridField.UpdateGridSize(x, y);

			gridCellsInternal = new Dictionary<Vector2Int, IBlockEntity>(x * y);
			
			for (var i = 0; i < x; i++) 
			for (var j = 0; j < y; j++)
				gridCellsInternal.Add(new Vector2Int(i, j), null);
		}
		
		public Vector2Int GetBlockCellToSwipe(IBlockEntity blockEntity, SwipeDirectionData swipeDirectionData)
			=> blocksOnGridField.GetValueOrDefault(blockEntity) + swipeDirectionData.directionVector;
		
		public Vector3 CalculateBlockLocalPositionOnGridCell(Vector2Int cell)
			=> new(CalculateCellSizeOffset(cell.x), CalculateCellSizeOffset(cell.y), 0f);

		public bool IsCellBusy(Vector2Int cell)
			=> busyCells.Contains(cell) || busyCellsByKill.Contains(cell);

		public void SetCellBusy(Vector2Int blockCell)
			=> busyCells.Add(blockCell);

		public void SetCellUnBusy(Vector2Int blockCell)
			=> busyCells.Remove(blockCell);

		public void SetCellBusyByKill(Vector2Int blockCell)
			=> busyCellsByKill.Add(blockCell);

		public void SetCellUnBusyByKill(Vector2Int blockCell)
			=> busyCellsByKill.Remove(blockCell);

		public void RemoveBlockFromGrid(IBlockEntity blockEntity, Vector2Int blockCell)
		{
			gridCellsInternal[blockCell] = null;
			blocksOnGridFieldInternal.Remove(blockEntity);
			
			OnBlockOnGridChanged?.Invoke(blockCell, null);
		}

		public void SetBlockOnGrid(IBlockEntity blockEntity, Vector2Int cell)
		{
			blocksOnGridFieldInternal[blockEntity] = cell;
			gridCellsInternal[cell] = blockEntity;
			
			OnBlockOnGridChanged?.Invoke(cell, blockEntity);
		}

		public void RemoveBlockFromCell(Vector2Int sourceCell)
		{
			gridCellsInternal[sourceCell] = null;
			OnBlockOnGridChanged?.Invoke(sourceCell, null);
		}

		public void AddKilledBlock(IBlockEntity blockEntity)
			=> killedBlocksInternal.Add(blockEntity);

		public void RemoveKilledBlocks(IBlockEntity blockEntity)
			=> killedBlocksInternal.Remove(blockEntity);
		
		private float CalculateCellSizeOffset(int y)
			=> y * gridField.cellSize + gridField.cellSize / 2;
	}
}