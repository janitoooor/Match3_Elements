using Core.Blocks;
using UnityEngine;

namespace Core.BlocksMovements
{
	public sealed class BlockMovementFactory : IBlockMovementFactory
	{
		public IBlockMovement Create()
			=> new BlockMovement();
		
		private sealed class BlockMovement : IBlockMovement
		{
			private MovedBlockDelegate finishedCallback;
			
			public IBlockEntity blockEntity { get; private set; }
			public Vector3 startPosition { get; private set; }
			public Vector3 targetPosition { get; private set; }
			public float elapsedTime { get; private set; }
			public bool isFall { get; private set; }
			public float moveDuration => blockEntity.moveDuration;
			
			public void Init(IBlockEntity block, Vector3 targetPos, bool isFallen, MovedBlockDelegate callback)
			{
				isFall = isFallen;
				blockEntity = block;
				targetPosition = targetPos;
				finishedCallback = callback;

				startPosition = block.GetLocalPosition();
				elapsedTime = 0f;
			}
			
			public void IncreaseElapsedTime(float increaseDelta)
				=> elapsedTime += increaseDelta;

			public void SetBlockLocalPosition(Vector3 blockLocalPos)
				=> blockEntity.SetLocalPosition(blockLocalPos);
			
			public void FinishMovement()
				=> finishedCallback?.Invoke(blockEntity);
		}
	}
}