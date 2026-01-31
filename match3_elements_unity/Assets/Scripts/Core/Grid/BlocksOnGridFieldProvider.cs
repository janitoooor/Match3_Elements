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
			
			SafeMoveBlockToCell(blockEntity, sourceNeighbourCell);
			SafeMoveBlockToCell(neighbourBlockEntity, sourceCell);
		}
		
		private void SafeMoveBlockToCell(IBlockEntity blockEntity, Vector2Int cellToSwap, 
			MovedBlockDelegate callback = null)
		{
			busyCells.Add(cellToSwap);
			blockMovementProcessor.MoveBlockTo(
				blockEntity, 
				CalculateBlockLocalPositionOnGridCell(cellToSwap), 
				MovedBlockFinishedCallback(cellToSwap, callback));
		}

		private MovedBlockDelegate MovedBlockFinishedCallback(Vector2Int cellToSwap, MovedBlockDelegate callback)
			=> movedBlockEntity =>
			{
				callback?.Invoke(movedBlockEntity);
				busyCells.Remove(cellToSwap);
			};

		private MovedBlockDelegate TryFallBlockAfterSingleMove(Vector2Int sourceCell, Vector2Int finishCell)
			=> movedBlockEntity =>
			{
				var downNeighbourCellY = finishCell.y - 1;

				if (downNeighbourCellY >= 0)
					TryFallBlockAfterMovedSide(finishCell, downNeighbourCellY, movedBlockEntity);
				
				var upNeighbourCellY = sourceCell.y + 1;

				if (upNeighbourCellY < gridField.gridSize.y)
					TryFallUpNeighbourAfterBlockMovedSide(sourceCell, upNeighbourCellY);
			};

		private void TryFallBlockAfterMovedSide(Vector2Int finishCell, int downNeighbourCellY, 
			IBlockEntity movedBlockEntity)
		{
			var cellToFall = new Vector2Int(finishCell.x, downNeighbourCellY);

			if (gridCells[cellToFall] == null)
				SingleMoveBlockToCell(movedBlockEntity, finishCell, cellToFall);
		}

		private void TryFallUpNeighbourAfterBlockMovedSide(Vector2Int sourceCell, int upNeighbourCellY)
		{
			var upNeighbourCell = new Vector2Int(sourceCell.x, upNeighbourCellY);
			var upNeighbourBlock = gridCells[upNeighbourCell];
				
			if (upNeighbourBlock != null && gridCells[sourceCell] == null)
				SingleMoveBlockToCell(upNeighbourBlock, upNeighbourCell, sourceCell);
		}

		private void SingleMoveBlockToCell(IBlockEntity blockEntity, Vector2Int sourceCell, Vector2Int cellToSwap)
		{
			gridCells[sourceCell] = null;
			
			gridCells[cellToSwap] = blockEntity;
			blocksOnGrid[blockEntity] = cellToSwap;
			
			SafeMoveBlockToCell(blockEntity, cellToSwap, TryFallBlockAfterSingleMove(sourceCell, cellToSwap));
		}

		private Vector2Int GetBlockCellPos(IBlockEntity blockEntity)
			=> blocksOnGrid.GetValueOrDefault(blockEntity);
		
		private Vector3 CalculateBlockLocalPositionOnGridCell(Vector2Int cell)
			=> new(
				CalculateCellSizeOffset(cell.x), 
				CalculateCellSizeOffset(cell.y),
				CalculateZOffset(cell.x, cell.y));

		private static float CalculateZOffset(int x, int y)
		{
			const float zOffsetCellY = 0.001f;
			const float zOffsetCellX = 0.00005f;
			return -(y * zOffsetCellY + x * zOffsetCellX);
		}

		private float CalculateCellSizeOffset(int y)
			=> y * gridField.cellSize + gridField.cellSize / 2;
	}
}