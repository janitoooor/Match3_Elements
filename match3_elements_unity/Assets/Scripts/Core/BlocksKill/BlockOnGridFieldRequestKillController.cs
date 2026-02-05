using System.Collections.Generic;
using Core.Blocks;
using UnityEngine;
using Zenject;

namespace Core.BlocksKill
{
	public sealed class BlockOnGridFieldRequestKillController
	{
		private readonly IBlockOnGridFieldRequestKillHandler handler;

		[Inject]
		public BlockOnGridFieldRequestKillController(
			IBlockOnGridFieldRequestKillHandler handler,
			IEnumerable<IBlockOnGridFieldRequestKillEvent> blockOnGridFieldRequestKillEvents)
		{
			this.handler = handler;

			foreach (var requestKillEvent in blockOnGridFieldRequestKillEvents)
				requestKillEvent.OnBlockOnGridFieldRequestKill += OnBlockOnGridFieldRequestKill;
		}

		private void OnBlockOnGridFieldRequestKill(IBlockEntity block, Vector2Int blockCell)
			=> handler.AddRequestForKillBlock(blockCell, block);
	}
}