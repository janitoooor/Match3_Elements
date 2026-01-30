using Core.Animation.Configs;
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

		private IAnimationSkinData animationSkinData;
		
		public void Setup(IAnimationSkinData skinData)
			=> animationSkinData = skinData;
		
		public void PlaceAt(Transform transformParent, Vector3 localPosition)
		{
			transform.SetParent(transformParent);
			transform.localPosition = localPosition;
			gameObject.SetActive(true);
			animationPlayer.PlayAnimation(animationSkinData.GetAnimationData(AnimationType.Idle));
		}

		public void KillBlock()
			=> animationPlayer.PlayAnimation(animationSkinData.GetAnimationData(AnimationType.Dead), DeadAnimationCallback());

		private AnimationFinishedDelegate DeadAnimationCallback()
			=> () =>
			{
				gameObject.SetActive(false);
				OnBlockEntityDead?.Invoke(this);
			};
	}
}