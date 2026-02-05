using Core.Blocks;
using UnityEngine;

namespace Core.BlocksKill
{
	public interface IBlocksOnGridFieldKiller
	{
		void TryKillBlocksInLine(IBlockEntity block, Vector2Int blockCell);
	}
}