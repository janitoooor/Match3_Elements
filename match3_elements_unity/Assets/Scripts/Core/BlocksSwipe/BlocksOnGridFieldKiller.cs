using System.Collections.Generic;
using Core.Blocks;
using Core.Grid;
using UnityEngine;
using Zenject;

namespace Core.BlocksSwipe
{
	public sealed class BlocksOnGridFieldKiller : IAllBlocksOnGridKilledEvent
	{
		public event AllBlocksOnGridKilledDelegate OnAllBlocksOnGridKilled;
		
		private readonly IBlocksOnGridRepository blocksOnGridRepository;
		private readonly IBlocksOnGridFieldMover blocksOnGridFieldMover;

		[Inject]
		public BlocksOnGridFieldKiller(
			IBlocksOnGridRepository blocksOnGridRepository, 
			IBlocksOnGridFieldMover blocksOnGridFieldMover)
		{
			this.blocksOnGridRepository = blocksOnGridRepository;
			this.blocksOnGridFieldMover = blocksOnGridFieldMover;

			blocksOnGridFieldMover.OnKillBlocksInLineRequest += TryKillBlocksInLine;
		}

		private void TryKillBlocksInLine(IBlockEntity block, Vector2Int blockCell)
		{ 
			HashSet<IBlockEntity> markedBlocsForKill = new();
			
			var horizontalLines = FindMatchingBlocksInLine(block, blockCell, true);
			var verticalLines = FindMatchingBlocksInLine(block, blockCell, false);
			
			MarkBlocksForKillInLines(horizontalLines, markedBlocsForKill);
			MarkBlocksForKillInLines(verticalLines, markedBlocsForKill);
    
			MarkBlocksNeighboursForKillInKilledLines(block,markedBlocsForKill, horizontalLines, true);
			MarkBlocksNeighboursForKillInKilledLines(block, markedBlocsForKill, verticalLines, false);
			
			KillAllMarkedBlocks(markedBlocsForKill);
			
			markedBlocsForKill.Clear();
		}
		
		private void MarkBlocksNeighboursForKillInKilledLines(IBlockEntity originalBlock, 
			HashSet<IBlockEntity> markedBlocsForKill, List<List<IBlockEntity>> blocksLines, bool isHorizontalLine)
		{
			foreach (var blockLine in blocksLines)
			foreach (var matchBlock in blockLine)
				FindAndAddNeighborsInDirection(
					originalBlock, 
					markedBlocsForKill, 
					blocksOnGridRepository.blocksOnGridField[matchBlock], 
					isHorizontalLine);
		}

		private void FindAndAddNeighborsInDirection(IBlockEntity block, HashSet<IBlockEntity> markedBlocsForKill, 
			Vector2Int startCell, bool isHorizontalLine)
		{
			if (isHorizontalLine)
				TryMarkForKillNeighboursInVerticalDirection(block, markedBlocsForKill, startCell);
			else
				TryMarkForKillNeighboursInHorizontalDirection(block, markedBlocsForKill, startCell);
		}

		private void TryMarkForKillNeighboursInVerticalDirection(IBlockEntity block, 
			HashSet<IBlockEntity> markedBlocsForKill, Vector2Int startCell)
		{
			for (var i = startCell.y + 1; i < blocksOnGridRepository.gridSize.y; i++)
				if (!TryMarkForKillNeighbour(block, markedBlocsForKill, startCell.x, i))
					break;

			for (var i = startCell.y - 1; i >= 0; i--)
				if (!TryMarkForKillNeighbour(block, markedBlocsForKill, startCell.x, i))
					break;
		}
		
		private void TryMarkForKillNeighboursInHorizontalDirection(IBlockEntity block, 
			HashSet<IBlockEntity> markedBlocsForKill, Vector2Int startCell)
		{
			for (var i = startCell.x + 1; i < blocksOnGridRepository.gridSize.x; i++)
				if (!TryMarkForKillNeighbour(block, markedBlocsForKill, i, startCell.y))
					break;

			for (var i = startCell.x - 1; i >= 0; i--)
				if (!TryMarkForKillNeighbour(block, markedBlocsForKill, i, startCell.y))
					break;
		}

		private bool TryMarkForKillNeighbour(IBlockEntity block, HashSet<IBlockEntity> markedBlocsForKill, int x, int y)
		{
			var neighbourCell = new Vector2Int(x, y);
			
			var neighbourBlock = blocksOnGridRepository.gridCells[neighbourCell];
			
			if (IsBlockMatch(neighbourBlock, block))
				markedBlocsForKill.Add(neighbourBlock);
			else
				return false;
			
			return true;
		}

		private List<List<IBlockEntity>> FindMatchingBlocksInLine(IBlockEntity block, Vector2Int blockCell, 
			bool isHorizontal)
		{
			var blocksLines = new List<List<IBlockEntity>> { new() };
			var gridSize = isHorizontal ? blocksOnGridRepository.gridSize.x : blocksOnGridRepository.gridSize.y;
    
			for (var i = 0; i < gridSize; i++)
			{
				var neighbourCell = isHorizontal 
					? new Vector2Int(i, blockCell.y) 
					: new Vector2Int(blockCell.x, i);
        
				var neighbourBlock = blocksOnGridRepository.gridCells[neighbourCell];
				
				var currentLine = blocksLines[^1];
        
				if (block == neighbourBlock || IsBlockMatch(neighbourBlock, block))
					currentLine.Add(neighbourBlock);
				else if (currentLine.Count > 0)
					blocksLines.Add(new List<IBlockEntity>());
			}
    
			return blocksLines;
		}
		
		private void MarkBlocksForKillInLines(List<List<IBlockEntity>> blocksLines, 
			HashSet<IBlockEntity> markedBlocsForKill)
		{
			for (var i = blocksLines.Count - 1; i >= 0; i--)
			{
				if (!TryAddLineToKillList(blocksLines[i], markedBlocsForKill))
					blocksLines.RemoveAt(i);
			}
		}
		
		private bool TryAddLineToKillList(List<IBlockEntity> blockLine, HashSet<IBlockEntity> markedBlocsForKill)
		{
			if (blockLine.Count < 3)
				return false;
			
			foreach (var blockEntity in blockLine)
				markedBlocsForKill.Add(blockEntity);

			return true;
		}

		private bool IsBlockMatch(IBlockEntity blockToCheck, IBlockEntity targetBlock)
			=> blockToCheck != null 
			   && !blocksOnGridFieldMover.IsBlockUnAvailable(targetBlock)
			   && blockToCheck.blockSkin == targetBlock.blockSkin;

		private void KillAllMarkedBlocks(HashSet<IBlockEntity> markedBlocsForKill)
		{
			foreach (var blockEntity in markedBlocsForKill)
				KillMarkedBlock(blockEntity);
		}

		private void KillMarkedBlock(IBlockEntity blockEntity)
		{
			var killedBlockCell = blocksOnGridRepository.blocksOnGridField[blockEntity];

			for (var y = killedBlockCell.y; y < blocksOnGridRepository.gridSize.y; y++)
				blocksOnGridRepository.SetCellBusyByKill(new Vector2Int(killedBlockCell.x, y));
			
			blocksOnGridRepository.AddKilledBlock(blockEntity);
        
			blockEntity.KillBlock(() => OnBlockKilled(blockEntity, killedBlockCell));
		}

		private void OnBlockKilled(IBlockEntity blockEntity, Vector2Int killedBlockCell)
		{
			for (var y = killedBlockCell.y; y < blocksOnGridRepository.gridSize.y; y++)
				blocksOnGridRepository.SetCellUnBusyByKill(new Vector2Int(killedBlockCell.x, y));
			
			blocksOnGridRepository.RemoveBlockFromGrid(blockEntity, killedBlockCell);
			
			blocksOnGridRepository.RemoveKilledBlocks(blockEntity);

			if (blocksOnGridRepository.blocksOnGridField.Count == 0)
				OnAllBlocksOnGridKilled?.Invoke();
			else
				blocksOnGridFieldMover.TryFallUpNeighboursAfterBlockMoved(killedBlockCell);
		}
	}
}