using System.Collections.Generic;
using System.Linq;
using Core.Blocks;
using UnityEngine;
using Zenject;

namespace Core.BlocksMovements
{
	public sealed class BlockMovementProcessor : IBlockMovementProcessor, ITickable
	{
		private readonly IBlockMovementsPool blockMovementsPool;

		private readonly List<IBlockMovement> activeMovements = new();		
		
		[Inject]
		public BlockMovementProcessor(IBlockMovementsPool blockMovementsPool)
			=> this.blockMovementsPool = blockMovementsPool;

		public void MoveBlockTo(IBlockEntity block, Vector3 targetPosition, bool isFallen, MovedBlockDelegate finishedCallback)
		{
			var movement = blockMovementsPool.GetBlockMovement();
			movement.Init(block, targetPosition, isFallen, finishedCallback);
			activeMovements.Insert(0, movement);
		}

		public bool IsBlockInMovement(IBlockEntity blockEntity)
			=> activeMovements.Any(movement => movement.blockEntity == blockEntity);

		public bool AnyBlocksIsFall()
			=> activeMovements.Any(a => a.isFall);

		public void Tick()
		{
			for (var i = activeMovements.Count - 1; i >= 0; i--)
				TickActiveBlockMovementAt(i);
		}

		private void TickActiveBlockMovementAt(int movementIndex)
		{
			var movement = activeMovements[movementIndex];
			movement.IncreaseElapsedTime(Time.deltaTime);
            
			var t = Mathf.Clamp01(movement.elapsedTime / movement.moveDuration);
			movement.SetBlockLocalPosition(Vector3.Lerp(movement.startPosition, movement.targetPosition, t));
            
			if (movement.elapsedTime >= movement.moveDuration)
				FinishBlockMovement(movement, movementIndex);
		}

		private void FinishBlockMovement(IBlockMovement movement, int i)
		{
			activeMovements.RemoveAt(i);
			
			movement.SetBlockLocalPosition(movement.targetPosition);
			movement.FinishMovement();
			
			blockMovementsPool.AddBlockMovement(movement);
		}
	}
}