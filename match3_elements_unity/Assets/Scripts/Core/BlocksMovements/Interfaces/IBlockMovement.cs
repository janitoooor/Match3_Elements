using Core.Blocks;
using UnityEngine;

namespace Core.BlocksMovements
{
	public delegate void MovedBlockDelegate(IBlockEntity blockEntity);
	
	public interface IBlockMovement
	{
		IBlockEntity blockEntity { get; }
		Vector3 startPosition { get; }
		Vector3 targetPosition { get; }
		float elapsedTime { get; }
		float moveDuration { get; }
		void Init(IBlockEntity block, Vector3 targetPos, MovedBlockDelegate callback);
		void IncreaseElapsedTime(float increaseDelta);
		void SetBlockLocalPosition(Vector3 blockLocalPos);
		void FinishMovement();
	}
}