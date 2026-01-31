using Core.Blocks;
using UnityEngine;

namespace Core.BlocksMovements
{
	public interface IBlockMovementProcessor
	{
		void MoveBlockTo(IBlockEntity block, Vector3 targetPosition, MovedBlockDelegate finishedCallback);
	}
}