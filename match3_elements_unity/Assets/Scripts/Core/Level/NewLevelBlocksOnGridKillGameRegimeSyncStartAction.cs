using System.Linq;
using Base;
using Core.Blocks;
using Core.BlocksKill;
using Core.Enums;
using Core.Grid;
using UnityEngine;
using Zenject;

namespace Core.Level
{
	public sealed class NewLevelBlocksOnGridKillGameRegimeSyncStartAction : GameRegimeSyncStartAction, 
		IBlockOnGridFieldRequestKillEvent
	{
		public event BlockOnGridFieldRequestKillDelegate OnBlockOnGridFieldRequestKill;
		
		private readonly IBlocksOnGridRepository blocksOnGridRepository;
		
		public override byte priority => (int)CoreGameRegimeSyncStartActionPriority.NewLevelBlocksOnGridKill;

		[Inject]
		public NewLevelBlocksOnGridKillGameRegimeSyncStartAction(IBlocksOnGridRepository blocksOnGridRepository)
			=> this.blocksOnGridRepository = blocksOnGridRepository;

		public override void Perform()
		{
			foreach (var blockOnGridField in blocksOnGridRepository.GetSaveBlocksOnGridField())
				TryKillBlock(blockOnGridField.Key, blockOnGridField.Value);
		}

		private void TryKillBlock(IBlockEntity blockEntity, Vector2Int blockCell)
		{
			if (!blocksOnGridRepository.killedBlocks.Contains(blockEntity))
				OnBlockOnGridFieldRequestKill?.Invoke(blockEntity, blockCell);
		}
	}
}