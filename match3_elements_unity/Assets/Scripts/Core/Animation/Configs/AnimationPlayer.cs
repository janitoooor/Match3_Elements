using System.Collections;
using Core.Animation.Enums;
using UnityEngine;

namespace Core.Animation.Configs
{
	public delegate void AnimationFinishedDelegate();
	
	public sealed class AnimationPlayer : MonoBehaviour
	{
		[SerializeField]
		private AnimationsContainer animationsContainer;
		
		[SerializeField]
		private SpriteRenderer animationSpriteRenderer;

		private Coroutine animationCoroutine;

		private IAnimationSkinData animationSkinData;
		
		private void Awake()
		{
			animationSkinData = animationsContainer.GetAnimationSkinData(AnimationSkin.Fire);

			PlayAnimation(animationSkinData.GetAnimationData(AnimationType.Idle));
		}

		private void OnMouseDown()
			=> PlayAnimation(animationSkinData.GetAnimationData(AnimationType.Destroy), ()=> gameObject.SetActive(false));

		public void PlayAnimation(IAnimationData animationData, AnimationFinishedDelegate finishedCallback = null)
		{
			if (animationCoroutine != null)
				StopCoroutine(animationCoroutine);
				
			animationCoroutine = StartCoroutine(AnimationsRoutine(animationData, finishedCallback));
		}

		private IEnumerator AnimationsRoutine(IAnimationData animationData, AnimationFinishedDelegate finishedCallback)
		{
			var spriteIndex = 0;
			
			while (spriteIndex < animationData.animationSprites.Count)
			{
				animationSpriteRenderer.sprite = animationData.animationSprites[spriteIndex];
				
				for (var i = 0; i < animationData.animationFrameRate; i++)
					yield return null;
				
				spriteIndex++;

				if (animationData.isLoopAnimation && spriteIndex >= animationData.animationSprites.Count)
					spriteIndex = 0;
			}
			
			finishedCallback?.Invoke();
		}
	}
}