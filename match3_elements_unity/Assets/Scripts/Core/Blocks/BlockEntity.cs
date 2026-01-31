using Core.Animation.Configs;
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
		
		private IAnimationSkinData animationSkinData;
		
		public void Setup(IAnimationSkinData skinData)
			=> animationSkinData = skinData;
		
		public void PlaceAt(Transform transformParent, Vector3 localPosition)
		{
			transform.SetParent(transformParent);
			SetLocalPosition(localPosition);
			
			gameObject.SetActive(true);
			animationPlayer.PlayAnimation(blockSkinData.GetAnimationData(AnimationType.Idle));
		}

		public void KillBlock()
			=> animationPlayer.PlayAnimation(animationSkinData.GetAnimationData(AnimationType.Dead), DeadAnimationCallback());

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