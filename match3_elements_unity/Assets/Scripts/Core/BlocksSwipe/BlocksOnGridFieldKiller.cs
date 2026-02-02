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

		private readonly Stack<IBlockEntity> tempConnectedBlocsStack = new();
		
		
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
    
			MarkBlocksNeighboursForKillInKilledLines(markedBlocsForKill, horizontalLines);
			MarkBlocksNeighboursForKillInKilledLines(markedBlocsForKill, verticalLines);
			
			KillAllMarkedBlocks(markedBlocsForKill);
			
			markedBlocsForKill.Clear();
		}
		
		private void MarkBlocksNeighboursForKillInKilledLines(HashSet<IBlockEntity> markedBlocsForKill, 
			List<List<IBlockEntity>> blocksLines)
		{
			foreach (var blockLine in blocksLines)
				foreach (var matchBlock in blockLine)
					FindAllConnectedBlocks(matchBlock, markedBlocsForKill);
		}
		
		private void FindAllConnectedBlocks(IBlockEntity startBlock, HashSet<IBlockEntity> markedBlocsForKill)
		{
			tempConnectedBlocsStack.Push(startBlock);
    
			while (tempConnectedBlocsStack.Count > 0)
			{
				var currentBlock = tempConnectedBlocsStack.Pop();
        
				if (startBlock != currentBlock && !markedBlocsForKill.Add(currentBlock))
					continue;
				
				var currentCell = blocksOnGridRepository.blocksOnGridField[currentBlock];
            
				CheckAndAddNeighbor(currentBlock, currentCell, 0, 1, markedBlocsForKill);
				CheckAndAddNeighbor(currentBlock, currentCell, 0, -1, markedBlocsForKill);
				CheckAndAddNeighbor(currentBlock, currentCell, 1, 0, markedBlocsForKill);
				CheckAndAddNeighbor(currentBlock, currentCell, -1, 0, markedBlocsForKill);
			}
			
			tempConnectedBlocsStack.Clear();
		}
			
		private void CheckAndAddNeighbor(IBlockEntity currentBlock, Vector2Int currentCell, 
			int dx, int dy, HashSet<IBlockEntity> markedBlocsForKill)
		{
			var neighborCell = new Vector2Int(currentCell.x + dx, currentCell.y + dy);
    
			if (neighborCell.x < 0 || neighborCell.x >= blocksOnGridRepository.gridSize.x ||
			    neighborCell.y < 0 || neighborCell.y >= blocksOnGridRepository.gridSize.y)
				return;
        
			var neighborBlock = blocksOnGridRepository.gridCells[neighborCell];
    
			if (neighborBlock != null && 
			    !markedBlocsForKill.Contains(neighborBlock) && 
			    IsBlockMatch(neighborBlock, currentBlock))
				tempConnectedBlocsStack.Push(neighborBlock);
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