using System.Collections;
using Core.Animation.Configs;
using Core.Enums;
using Core.Grid;
using UnityEngine;

namespace Core.Blocks
{
	public delegate void BlockEntityDeadDelegate(IBlockEntity blockEntity);
	
	public sealed class BlockEntity : MonoBehaviour, IBlockEntity
	{
		public event BlockEntityDeadDelegate OnBlockEntityDead;
		
		[SerializeField] 
		private float moveDuration = 0.5f;
		
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

		public void MoveTo(Vector3 localPosition, MovedBlockDelegate callback)
			=> StartCoroutine(MoveToPosition(localPosition, callback));

		public void KillBlock()
			=> animationPlayer.PlayAnimation(animationSkinData.GetAnimationData(AnimationType.Dead), DeadAnimationCallback());

		private AnimationFinishedDelegate DeadAnimationCallback()
			=> () =>
			{
				gameObject.SetActive(false);
				OnBlockEntityDead?.Invoke(this);
			};

		private IEnumerator MoveToPosition(Vector3 targetPosition, MovedBlockDelegate callback)
		{
			var startPosition = transform.localPosition;
			var elapsedTime = 0f;

			while (elapsedTime < moveDuration)
			{
				transform.localPosition = Vector3.Lerp(startPosition, targetPosition, Mathf.Clamp01(elapsedTime / moveDuration));
				elapsedTime += Time.deltaTime;
				yield return null;
			}
        
			transform.localPosition = targetPosition;
        
			callback?.Invoke(this);
		}
	}
}