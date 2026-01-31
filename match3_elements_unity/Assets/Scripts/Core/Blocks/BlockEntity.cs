using Core.Animation;
using Core.Enums;
using UnityEngine;

namespace Core.Blocks
{
	public delegate void BlockEntityDeadDelegate(IBlockEntity blockEntity);
	
	public sealed class BlockEntity : MonoBehaviour, IBlockEntity
	{
		public event BlockEntityDeadDelegate OnBlockEntityDead;
		
		[field: SerializeField] 
		public float moveDuration { get; private set; } = 0.5f;
		
		[SerializeField]
		private AnimationPlayer animationPlayer;
		
		private IBlockSkinData blockSkinData;
		
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

		public void KillBlock()
			=> animationPlayer.PlayAnimation(
				blockSkinData.GetAnimationData(AnimationType.Dead), 
				DeadAnimationCallback());

		public Vector3 GetLocalPosition()
			=> transform.localPosition;

		public void SetLocalPosition(Vector3 localPosition)
			=> transform.localPosition = localPosition;

		private AnimationFinishedDelegate DeadAnimationCallback()
			=> () =>
			{
				gameObject.SetActive(false);
				OnBlockEntityDead?.Invoke(this);
			};
	}
}