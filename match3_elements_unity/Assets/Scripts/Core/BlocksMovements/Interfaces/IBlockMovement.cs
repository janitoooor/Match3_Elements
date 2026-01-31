using Core.Blocks;
using UnityEngine;

namespace Core.BlocksMovements
{
	public delegate void MovedBlockDelegate(IBlockEntity blockEntity);
	
	public interface IBlockMovement
	{
		Vector3 startPosition { get; }
		Vector3 targetPosition { get; }
		float elapsedTime { get; }
		float moveDuration { get; }
		void Init(IBlockEntity blockEntity, Vector3 targetPos, MovedBlockDelegate finishedCallback);
		void IncreaseElapsedTime(float increaseDelta);
		void SetBlockLocalPosition(Vector3 blockLocalPos);
		void FinishMovement();
	}
}