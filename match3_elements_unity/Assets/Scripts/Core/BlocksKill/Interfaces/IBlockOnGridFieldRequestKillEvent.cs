using Core.Blocks;
using UnityEngine;

namespace Core.BlocksKill
{
	public delegate void BlockOnGridFieldRequestKillDelegate(IBlockEntity block, Vector2Int blockCell);
	
	public interface IBlockOnGridFieldRequestKillEvent
	{
		event BlockOnGridFieldRequestKillDelegate OnBlockOnGridFieldRequestKill;
	}
}