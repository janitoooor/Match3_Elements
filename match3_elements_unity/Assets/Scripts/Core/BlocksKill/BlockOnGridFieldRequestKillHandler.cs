using System.Collections.Generic;
using System.Linq;
using Core.Blocks;
using Core.BlocksMovements;
using Core.Grid;
using UnityEngine;
using Zenject;

namespace Core.BlocksKill
{
	public sealed class BlockOnGridFieldRequestKillHandler : IBlockOnGridFieldRequestKillHandler
	{
		private readonly IBlocksOnGridFieldKiller blocksOnGridFieldKiller;
		private readonly IBlocksOnGridRepository blocksOnGridRepository;
		private readonly IBlockMovementProcessor blockMovementProcessor;
		
		private readonly Dictionary<IBlockEntity, Vector2Int> requestedBlocksForKill = new();

		[Inject]
		public BlockOnGridFieldRequestKillHandler(
			IBlocksOnGridFieldKiller blocksOnGridFieldKiller, 
			IBlocksOnGridRepository blocksOnGridRepository, 
			IBlockMovementProcessor blockMovementProcessor)
		{
			this.blocksOnGridFieldKiller = blocksOnGridFieldKiller;
			this.blocksOnGridRepository = blocksOnGridRepository;
			this.blockMovementProcessor = blockMovementProcessor;
		}

		public void AddRequestForKillBlock(Vector2Int targetCell, IBlockEntity movedBlockEntity)
		{
			TryAddBlockToDictForKill(targetCell, movedBlockEntity);
				
			if (!blockMovementProcessor.AnyBlocksIsFall())
				TryKillBlocks();
		}
		
		private void TryAddBlockToDictForKill(Vector2Int targetCell, IBlockEntity movedBlockEntity)
		{
			if (!requestedBlocksForKill.TryAdd(movedBlockEntity, targetCell))
				requestedBlocksForKill[movedBlockEntity] = targetCell;
		}

		private void TryKillBlocks()
		{
			foreach (var (blockEntity, movedBlockCell) in requestedBlocksForKill)
				TryKillBlock(blockEntity, movedBlockCell);

			requestedBlocksForKill.Clear();
		}

		private void TryKillBlock(IBlockEntity blockEntity, Vector2Int movedBlockCell)
		{
			if (!blocksOnGridRepository.killedBlocks.Contains(blockEntity))
				blocksOnGridFieldKiller.TryKillBlocksInLine(blockEntity, movedBlockCell);
		}
	}
}