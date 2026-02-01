using System.Collections.Generic;
using Core.Blocks;
using Core.BlocksMovements;
using Core.Enums;
using Core.Input;
using UnityEngine;
using Zenject;

namespace Core.Grid
{
	public sealed class BlocksOnGridFieldProvider : IBlocksOnGridFieldProvider
	{
		private readonly IGridField gridField;
		private readonly IBlockMovementProcessor blockMovementProcessor;
		
		private readonly Dictionary<IBlockEntity, Vector2Int> blocksOnGrid = new();
		private Dictionary<Vector2Int, IBlockEntity> gridCells;

		private readonly List<Vector2Int> busyCells = new();

		public IReadOnlyDictionary<IBlockEntity, Vector2Int> blocksOnGridField => blocksOnGrid;
		
		[Inject]
		public BlocksOnGridFieldProvider(IGridField gridField, IBlockMovementProcessor blockMovementProcessor)
		{
			this.gridField = gridField;
			this.blockMovementProcessor = blockMovementProcessor;
		}
		
		public void AddBlockOnGrid(IBlockEntity blockEntity, Vector2Int cell)
		{
			blockEntity.PlaceAt(
				gridField.GetTransformParentForBlocksInCell(), 
				CalculateBlockLocalPositionOnGridCell(cell));
			
			SetBlockRendererSortingOderForCell(blockEntity, cell);
			
			blocksOnGrid.Add(blockEntity, cell);
			gridCells[cell] = blockEntity;
		}
		
		public void SetGridSize(int x, int y)
		{
			gridField.UpdateGridSize(x, y);

			gridCells = new Dictionary<Vector2Int, IBlockEntity>(x * y);
			
			for (var i = 0; i < x; i++) 
				for (var j = 0; j < y; j++)
					gridCells.Add(new Vector2Int(i, j), null);
		}
		
		public Vector2Int GetBlockCellToSwipe(IBlockEntity blockEntity, SwipeDirectionData swipeDirectionData)
			=> GetBlockCellPos(blockEntity) + swipeDirectionData.directionVector;
		
		public void SwipeBlockTo(IBlockEntity blockEntity, Vector2Int cellToSwap, SwipeDirectionData swipeDirectionData)
		{
			if (busyCells.Contains(cellToSwap))
			{
				Debug.Log($"====> cell {cellToSwap} is busy!");
				return;
			}
			
			if (blockMovementProcessor.IsBlockInMovement(blockEntity))
			{
				Debug.Log($"====> block at loc pos {blockEntity.GetLocalPosition()} is in movement!");
				return;
			}
			
			var neighbourBlock = gridCells[cellToSwap];
			var sourceCell = blocksOnGrid[blockEntity];
			
			if (neighbourBlock != null)
				SwapBlockWithNeighbour(blockEntity, neighbourBlock, sourceCell, cellToSwap);
			else if (IsSwapAxisX(swipeDirectionData)) 
				SingleMoveBlockToCell(blockEntity, sourceCell, cellToSwap);
		}

		private static bool IsSwapAxisX(SwipeDirectionData swipeDirectionData)
			=> swipeDirectionData.directionType is SwipeDirectionType.Left or SwipeDirectionType.Right;

		private void SwapBlockWithNeighbour(IBlockEntity blockEntity, IBlockEntity neighbourBlockEntity,
			Vector2Int sourceCell, Vector2Int sourceNeighbourCell)
		{
			gridCells[sourceCell] = neighbourBlockEntity;
			gridCells[sourceNeighbourCell] = blockEntity;
			
			blocksOnGrid[blockEntity] = sourceNeighbourCell;
			blocksOnGrid[neighbourBlockEntity] = sourceCell;

			var blockSortingOrder = blockEntity.rendererSortingOrder;
			var neighbourBlockSortingOrder = neighbourBlockEntity.rendererSortingOrder;
			
			SetSourceBlockRendererSortingOderBeforeMove(blockEntity, neighbourBlockSortingOrder);
			neighbourBlockEntity.SetRendererSortingOder(Mathf.Min(blockSortingOrder, neighbourBlockSortingOrder) + 1);
			
			SafeMoveBlockToCell(blockEntity, sourceNeighbourCell);
			SafeMoveBlockToCell(neighbourBlockEntity, sourceCell);
		}

		private void SingleMoveBlockToCell(IBlockEntity blockEntity, Vector2Int sourceCell, Vector2Int targetCell)
		{
			gridCells[sourceCell] = null;
			
			gridCells[targetCell] = blockEntity;
			blocksOnGrid[blockEntity] = targetCell;
			
			var targetCellSortingOrder = CalculateBlockRendererSortingOrderForCell(targetCell.x, targetCell.y);
			SetSourceBlockRendererSortingOderBeforeMove(blockEntity, targetCellSortingOrder);
			
			SafeMoveBlockToCell(blockEntity, targetCell, TryFallBlockAfterSingleMove(sourceCell, targetCell));
		}
		
		private void SafeMoveBlockToCell(IBlockEntity blockEntity, Vector2Int targetCell, 
			MovedBlockDelegate callback = null)
		{
			busyCells.Add(targetCell);
			
			blockMovementProcessor.MoveBlockTo(
				blockEntity, 
				CalculateBlockLocalPositionOnGridCell(targetCell), 
				MovedBlockFinishedCallback(targetCell, callback));
		}

		private MovedBlockDelegate MovedBlockFinishedCallback(Vector2Int targetCell, MovedBlockDelegate callback)
			=> movedBlockEntity =>
			{
				SetBlockRendererSortingOderForCell(movedBlockEntity, targetCell);
				callback?.Invoke(movedBlockEntity);
				busyCells.Remove(targetCell);
			};

		private MovedBlockDelegate TryFallBlockAfterSingleMove(Vector2Int sourceCell, Vector2Int targetCell)
			=> movedBlockEntity =>
			{
				var downNeighbourCellY = targetCell.y - 1;

				if (downNeighbourCellY >= 0)
					TryFallBlockAfterMovedSide(targetCell, downNeighbourCellY, movedBlockEntity);
				
				var upNeighbourCellY = sourceCell.y + 1;

				if (upNeighbourCellY < gridField.gridSize.y)
					TryFallUpNeighbourAfterBlockMovedSide(sourceCell, upNeighbourCellY);
			};

		private void TryFallBlockAfterMovedSide(Vector2Int targetCell, int downNeighbourCellY, 
			IBlockEntity movedBlockEntity)
		{
			var cellToFall = new Vector2Int(targetCell.x, downNeighbourCellY);

			if (gridCells[cellToFall] == null)
				SingleMoveBlockToCell(movedBlockEntity, targetCell, cellToFall);
		}

		private void TryFallUpNeighbourAfterBlockMovedSide(Vector2Int sourceCell, int upNeighbourCellY)
		{
			var upNeighbourCell = new Vector2Int(sourceCell.x, upNeighbourCellY);
			var upNeighbourBlock = gridCells[upNeighbourCell];
				
			if (upNeighbourBlock != null && gridCells[sourceCell] == null)
				SingleMoveBlockToCell(upNeighbourBlock, upNeighbourCell, sourceCell);
		}

		private static void SetSourceBlockRendererSortingOderBeforeMove(IBlockEntity blockEntity, int finishSortingOrder)
			=> blockEntity.SetRendererSortingOder(Mathf.Max(blockEntity.rendererSortingOrder, finishSortingOrder) - 1);

		private static void SetBlockRendererSortingOderForCell(IBlockEntity blockEntity, Vector2Int cell)
			=> blockEntity.SetRendererSortingOder(CalculateBlockRendererSortingOrderForCell(cell.x, cell.y));

		private Vector2Int GetBlockCellPos(IBlockEntity blockEntity)
			=> blocksOnGrid.GetValueOrDefault(blockEntity);
		
		private Vector3 CalculateBlockLocalPositionOnGridCell(Vector2Int cell)
			=> new(CalculateCellSizeOffset(cell.x), CalculateCellSizeOffset(cell.y), 0f);

		private static int CalculateBlockRendererSortingOrderForCell(int x, int y)
		{
			const int cellSortingOrder = 10; 
			return cellSortingOrder * (x + y);
		}

		private float CalculateCellSizeOffset(int y)
			=> y * gridField.cellSize + gridField.cellSize / 2;
	}
}