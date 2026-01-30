using System.Collections.Generic;
using Core.Blocks;
using Core.Enums;
using Core.Input;
using UnityEngine;
using Zenject;

namespace Core.Grid
{
	public sealed class BlocksOnGridFieldProvider : IBlocksOnGridFieldProvider
	{
		private readonly IGridField gridField;
		
		private readonly Dictionary<IBlockEntity, Vector2Int> blocksOnGrid = new();
		private Dictionary<Vector2Int, IBlockEntity> gridCells;

		private readonly List<Vector2Int> busyCells = new();

		[Inject]
		public BlocksOnGridFieldProvider(IGridField gridField)
			=> this.gridField = gridField;

		public void SwipeBlockTo(IBlockEntity blockEntity, Vector2Int cellToSwap, SwipeDirectionData swipeDirectionData)
		{
			if (busyCells.Contains(cellToSwap))
			{
				Debug.Log($"====> cell {cellToSwap} is busy!");
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
		
		private void SingleMoveBlockToCell(IBlockEntity blockEntity, Vector2Int sourceCell, Vector2Int cellToSwap)
		{
			gridCells[sourceCell] = null;
			
			gridCells[cellToSwap] = blockEntity;
			blocksOnGrid[blockEntity] = cellToSwap;
			
			SafeMoveBlockToCell(blockEntity, cellToSwap, TryFallBlockAfterSingleMove(cellToSwap));
		}
		
		private void SafeMoveBlockToCell(IBlockEntity blockEntity, Vector2Int cellToSwap, 
			MovedBlockDelegate callback = null)
		{
			busyCells.Add(cellToSwap);
			gridField.MoveBlockToCell(blockEntity, cellToSwap, movedBlockEntity =>
			{
				callback?.Invoke(movedBlockEntity);
				busyCells.Remove(cellToSwap);
			});
		}

		private MovedBlockDelegate TryFallBlockAfterSingleMove(Vector2Int finishCell)
			=> movedBlockEntity =>
			{
				var downNeighbourCellY = finishCell.y - 1;

				var cellToFall = new Vector2Int(finishCell.x, downNeighbourCellY);
				
				if (downNeighbourCellY >= 0 && gridCells[cellToFall] == null)
					SingleMoveBlockToCell(movedBlockEntity, finishCell, cellToFall);
			};

		public Vector2Int GetBlockCellToSwipe(IBlockEntity blockEntity, SwipeDirectionData swipeDirectionData)
			=> GetBlockCellPos(blockEntity) + swipeDirectionData.directionVector;

		public void AddBlockOnGrid(IBlockEntity blockEntity, Vector2Int cellPos)
		{
			gridField.PlaceBlockAtCell(blockEntity, cellPos);
			
			blocksOnGrid.Add(blockEntity, cellPos);
			gridCells[cellPos] = blockEntity;
		}

		public void ChangeGridSize(int x, int y)
		{
			gridField.UpdateGridSize(x, y);

			gridCells = new Dictionary<Vector2Int, IBlockEntity>(x * y);
			
			for (var i = 0; i < x; i++)
				for (var j = 0; j < y; j++)
					gridCells.Add(new Vector2Int(i, j), null);
		}

		private Vector2Int GetBlockCellPos(IBlockEntity blockEntity)
			=> blocksOnGrid.GetValueOrDefault(blockEntity);
	}
}