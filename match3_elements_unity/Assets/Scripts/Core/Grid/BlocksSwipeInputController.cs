using Core.Blocks;
using Core.Input;
using UnityEngine;
using Zenject;

namespace Core.Grid
{
	public sealed class BlocksSwipeInputController
	{
		private readonly Camera camera;
		private readonly IBlocksOnGridSwipeModel blocksOnGridSwipeModel;

		private readonly int blockLayer = LayerMask.GetMask("Blocks");
		
		[Inject]
		public BlocksSwipeInputController(
			Camera camera, 
			IBlocksOnGridSwipeModel blocksOnGridSwipeModel, 
			ISwipeInputEvent swipeInputEvent)
		{
			this.camera = camera;
			this.blocksOnGridSwipeModel = blocksOnGridSwipeModel;
			
			swipeInputEvent.OnInputSwipe += OnInputSwipe;
		}
		
		private void OnInputSwipe(SwipeDirectionData swipeDirectionData, Vector2 startTouchScreenPosition, 
			Vector2 currentTouchScreenPosition)
		{
			var worldPoint = camera.ScreenToWorldPoint(startTouchScreenPosition);
			var hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, blockLayer);
			
			if (hit && TryGetBlockSwipeCollider(hit, out var blockSwapCollider))
				blocksOnGridSwipeModel.TrySwipeBlockTo(blockSwapCollider.blockEntity, swipeDirectionData);
		}
		
		private static bool TryGetBlockSwipeCollider(RaycastHit2D hit, out BlockSwipeCollider blockSwapCollider)
			=> hit.collider.gameObject.TryGetComponent(out blockSwapCollider);
	}
}