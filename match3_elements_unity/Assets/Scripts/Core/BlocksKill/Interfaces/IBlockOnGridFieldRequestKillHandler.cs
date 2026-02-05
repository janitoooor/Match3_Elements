using Core.Blocks;
using UnityEngine;

namespace Core.BlocksKill
{
	public interface IBlockOnGridFieldRequestKillHandler
	{
		void AddRequestForKillBlock(Vector2Int targetCell, IBlockEntity movedBlockEntity);
	}
}