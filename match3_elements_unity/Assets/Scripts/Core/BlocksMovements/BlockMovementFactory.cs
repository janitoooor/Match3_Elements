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
			private IBlockEntity block;
			private MovedBlockDelegate callback;
			public Vector3 startPosition { get; private set; }
			public Vector3 targetPosition { get; private set; }
			public float elapsedTime { get; private set; }
			public float moveDuration => block.moveDuration;
			
			public void Init(IBlockEntity blockEntity, Vector3 targetPos, MovedBlockDelegate finishedCallback)
			{
				block = blockEntity;
				targetPosition = targetPos;
				callback = finishedCallback;

				startPosition = blockEntity.GetLocalPosition();
				elapsedTime = 0f;
			}
			
			public void IncreaseElapsedTime(float increaseDelta)
				=> elapsedTime += increaseDelta;

			public void SetBlockLocalPosition(Vector3 blockLocalPos)
				=> block.SetLocalPosition(blockLocalPos);
			
			public void FinishMovement()
				=> callback?.Invoke(block);
		}
	}
}