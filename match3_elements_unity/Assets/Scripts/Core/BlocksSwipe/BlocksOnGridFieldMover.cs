using System.Linq;
using Core.Blocks;
using Core.BlocksMovements;
using Core.Enums;
using Core.Grid;
using Core.Input;
using UnityEngine;
using Zenject;

namespace Core.BlocksSwipe
{
	public sealed class BlocksOnGridFieldMover : IBlocksOnGridFieldMover
	{
		public event KillBlocksInLineRequestDelegate OnKillBlocksInLineRequest;
		
		private readonly IBlocksOnGridRepository blocksOnGridRepository;
		private readonly IBlockMovementProcessor blockMovementProcessor;
		private readonly IBlockOnGridRendererSortingOderProvider sortingOderProvider;

		[Inject]
		public BlocksOnGridFieldMover(
			IBlocksOnGridRepository blocksOnGridRepository, 
			IBlockMovementProcessor blockMovementProcessor,
			IBlockOnGridRendererSortingOderProvider sortingOderProvider)
		{
			this.blocksOnGridRepository = blocksOnGridRepository;
			this.blockMovementProcessor = blockMovementProcessor;
			this.sortingOderProvider = sortingOderProvider;
		}

		public void SwipeBlockTo(IBlockEntity blockEntity, Vector2Int cellToSwap, SwipeDirectionData swipeDirectionData)
		{
			if (blocksOnGridRepository.IsCellBusy(cellToSwap))
			{
				Debug.Log($"====> cell {cellToSwap} is busy!");
				return;
			}
			
			if (IsBlockUnAvailable(blockEntity))
			{
				Debug.Log($"====> block at loc pos {blockEntity.GetLocalPosition()} is not available!");
				return;
			}
			
			var sourceCell = blocksOnGridRepository.blocksOnGridField[blockEntity];
			
			if (blocksOnGridRepository.IsCellBusy(sourceCell))
			{
				Debug.Log($"====> cell {sourceCell} is busy!");
				return;
			}
			
			var neighbourBlock = blocksOnGridRepository.gridCells[cellToSwap];
			
			if (neighbourBlock != null)
				SwapBlockWithNeighbour(blockEntity, neighbourBlock, sourceCell, cellToSwap);
			else if (IsSwapAxisX(swipeDirectionData)) 
				SingleMoveBlockToCell(blockEntity, sourceCell, cellToSwap, false);
		}
		
		public void TryFallUpNeighboursAfterBlockMoved(Vector2Int sourceCell)
		{
			var upNeighbourCellY = sourceCell.y + 1;
			var isNeighbourFallen = true;
				
			while (upNeighbourCellY < blocksOnGridRepository.gridSize.y && isNeighbourFallen)
			{
				isNeighbourFallen = TryFallUpNeighbourAfterBlockMovedSide(sourceCell.x, upNeighbourCellY);
				upNeighbourCellY++;
			}
		}
		
		public bool IsBlockUnAvailable(IBlockEntity block)
			=> blockMovementProcessor.IsBlockInMovement(block) || blocksOnGridRepository.killedBlocks.Contains(block);

		private static bool IsSwapAxisX(SwipeDirectionData swipeDirectionData)
			=> swipeDirectionData.directionType is SwipeDirectionType.Left or SwipeDirectionType.Right;

		private void SwapBlockWithNeighbour(IBlockEntity blockEntity, IBlockEntity neighbourBlockEntity,
			Vector2Int sourceCell, Vector2Int sourceNeighbourCell)
		{
			blocksOnGridRepository.SetBlockOnGrid(neighbourBlockEntity, sourceCell);
			blocksOnGridRepository.SetBlockOnGrid(blockEntity, sourceNeighbourCell);

			var blockSortingOrder = blockEntity.rendererSortingOrder;
			var neighbourBlockSortingOrder = neighbourBlockEntity.rendererSortingOrder;
			
			sortingOderProvider.SetSourceBlockRendererSortingOderBeforeMove(
				blockEntity, 
				neighbourBlockSortingOrder, 
				sourceNeighbourCell.y,
				blocksOnGridRepository.gridSize.y);
			
			neighbourBlockEntity.SetRendererSortingOder(Mathf.Min(blockSortingOrder, neighbourBlockSortingOrder) + 1);
			
			SafeMoveBlockToCell(blockEntity, sourceNeighbourCell, false);
			SafeMoveBlockToCell(neighbourBlockEntity, sourceCell, false);
		}

		private void SingleMoveBlockToCell(IBlockEntity blockEntity, Vector2Int sourceCell, Vector2Int targetCell, 
			bool isFallen)
		{
			blocksOnGridRepository.RemoveBlockFromCell(sourceCell);
			
			blocksOnGridRepository.SetBlockOnGrid(blockEntity, targetCell);
			
			var targetCellSortingOrder = sortingOderProvider.CalculateBlockRendererSortingOrderForCell(
				targetCell.x, 
				targetCell.y);
			
			sortingOderProvider.SetSourceBlockRendererSortingOderBeforeMove(
				blockEntity, 
				targetCellSortingOrder, 
				targetCell.y, 
				blocksOnGridRepository.gridSize.y);
			
			SafeMoveBlockToCell(
				blockEntity, 
				targetCell,
				isFallen, 
				TryFallBlockAfterSingleMove(sourceCell, targetCell));
		}
		
		private void SafeMoveBlockToCell(IBlockEntity blockEntity, Vector2Int targetCell, bool isFallen,
			MovedBlockDelegate callback = null)
		{
			blocksOnGridRepository.SetCellBusy(targetCell);
			
			blockMovementProcessor.MoveBlockTo(
				blockEntity, 
				blocksOnGridRepository.CalculateBlockLocalPositionOnGridCell(targetCell), 
				isFallen,
				MovedBlockFinishedCallback(targetCell, callback));
		}

		private MovedBlockDelegate MovedBlockFinishedCallback(Vector2Int targetCell, MovedBlockDelegate callback)
			=> movedBlockEntity =>
			{
				sortingOderProvider.SetBlockRendererSortingOderForCell(movedBlockEntity, targetCell);
				blocksOnGridRepository.SetCellUnBusy(targetCell);
				callback?.Invoke(movedBlockEntity);
				
				if (!blockMovementProcessor.AnyBlocksIsFall())
					OnKillBlocksInLineRequest?.Invoke(movedBlockEntity, targetCell);
			};

		private MovedBlockDelegate TryFallBlockAfterSingleMove(Vector2Int sourceCell, Vector2Int targetCell) 
			=> movedBlockEntity =>
			{
				var downNeighbourCellY = targetCell.y - 1;

				if (downNeighbourCellY >= 0)
					TryFallBlockAfterMovedSide(targetCell, downNeighbourCellY, movedBlockEntity);

				TryFallUpNeighboursAfterBlockMoved(sourceCell);
			};
		
		private void TryFallBlockAfterMovedSide(Vector2Int targetCell, int downNeighbourCellY, IBlockEntity blockEntity)
		{
			var cellToFall = new Vector2Int(targetCell.x, downNeighbourCellY);

			var blockInCellToFall = blocksOnGridRepository.gridCells[cellToFall];
			
			if (blockInCellToFall == null)
				FallBlockAfterMovedSide(targetCell, blockEntity, cellToFall);
		}

		private void FallBlockAfterMovedSide(Vector2Int targetCell, IBlockEntity movedBlockEntity, Vector2Int cellToFall)
		{
			SingleMoveBlockToCell(movedBlockEntity, targetCell, cellToFall, true);
			TryFallUpNeighboursAfterBlockMoved(targetCell);
		}

		private bool TryFallUpNeighbourAfterBlockMovedSide(int sourceDownCellX, int upNeighbourCellY)
		{
			var upNeighbourCell = new Vector2Int(sourceDownCellX, upNeighbourCellY);
			var upNeighbourBlock = blocksOnGridRepository.gridCells[upNeighbourCell];

			var downCell = new Vector2Int(sourceDownCellX, upNeighbourCellY - 1);
			
			if (IsBlockUnAvailable(upNeighbourBlock) || upNeighbourBlock == null || blocksOnGridRepository.gridCells[downCell] != null)
				return false;

			SingleMoveBlockToCell(upNeighbourBlock, upNeighbourCell, downCell, true);
			return true;
		}
	}
}