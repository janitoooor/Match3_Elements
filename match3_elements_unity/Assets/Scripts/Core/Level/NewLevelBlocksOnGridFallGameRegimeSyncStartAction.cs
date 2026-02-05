using Base;
using Core.Blocks;
using Core.BlocksSwipe;
using Core.Enums;
using Core.Grid;
using UnityEngine;
using Zenject;

namespace Core.Level
{
	public sealed class NewLevelBlocksOnGridFallGameRegimeSyncStartAction : GameRegimeSyncStartAction
	{
		private readonly IBlocksOnGridRepository blocksOnGridRepository;
		private readonly IBlocksOnGridFieldMover blocksOnGridFieldMover;
		
		public override byte priority => (int)CoreGameRegimeSyncStartActionPriority.NewLevelBlocksOnGridFall;

		[Inject]
		public NewLevelBlocksOnGridFallGameRegimeSyncStartAction(
			IBlocksOnGridRepository blocksOnGridRepository,
			IBlocksOnGridFieldMover blocksOnGridFieldMover)
		{
			this.blocksOnGridRepository = blocksOnGridRepository;
			this.blocksOnGridFieldMover = blocksOnGridFieldMover;
		}

		public override void Perform()
		{
			foreach (var blockOnGridField in blocksOnGridRepository.GetSaveBlocksOnGridField())
				TryFallBlock(blockOnGridField.Key, blockOnGridField.Value);
		}

		private void TryFallBlock(IBlockEntity block, Vector2Int blockCell)
		{
			if (!blocksOnGridFieldMover.IsBlockUnAvailable(block))
				blocksOnGridFieldMover.TryFallBlockFrom(block, blockCell);
		}
	}
}