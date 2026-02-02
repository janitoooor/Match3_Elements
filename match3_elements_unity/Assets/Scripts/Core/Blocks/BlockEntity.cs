using Core.Animation;
using Core.Enums;
using UnityEngine;

namespace Core.Blocks
{
	public delegate void BlockEntityDeadDelegate(IBlockEntity blockEntity);
	
	public sealed class BlockEntity : MonoBehaviour, IBlockEntity
	{
		public event BlockEntityDeadDelegate OnBlockEntityDead;
		
		[SerializeField]
		private AnimationPlayer animationPlayer;
		
		[field: SerializeField] 
		public float moveDuration { get; private set; } = 0.5f;
		
		private IBlockSkinData blockSkinData;
		
		public int rendererSortingOrder => animationPlayer.rendererSortingOrder;

		public BlockSkin blockSkin => blockSkinData.blockSkin;
		
		public void SetRendererSortingOder(int sortingLayer)
			=> animationPlayer.SetRendererSortingOder(sortingLayer);

		public void Setup(IBlockSkinData skinData)
			=> blockSkinData = skinData;
		
		public void PlaceAt(Transform transformParent, Vector3 localPosition)
		{
			transform.SetParent(transformParent);
			SetLocalPosition(localPosition);
		}

		public void ShowBlock()
		{
			gameObject.SetActive(true);
			animationPlayer.PlayAnimation(blockSkinData.GetAnimationData(AnimationType.Idle));
		}

		public void KillBlock(KilledBlockDelegate killedCallback)
			=> animationPlayer.PlayAnimation(
				blockSkinData.GetAnimationData(AnimationType.Dead), 
				DeadAnimationCallback(killedCallback));

		public Vector3 GetLocalPosition()
			=> transform.localPosition;

		public void SetLocalPosition(Vector3 localPosition)
			=> transform.localPosition = localPosition;

		private AnimationFinishedDelegate DeadAnimationCallback(KilledBlockDelegate killedCallback)
			=> () =>
			{
				gameObject.SetActive(false);
				OnBlockEntityDead?.Invoke(this);
				killedCallback?.Invoke();
			};
	}
}