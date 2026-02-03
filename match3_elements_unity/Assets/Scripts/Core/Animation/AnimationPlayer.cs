using System.Collections;
using Core.Blocks;
using UnityEngine;

namespace Core.Animation
{
	public delegate void AnimationFinishedDelegate();
	
	public sealed class AnimationPlayer : MonoBehaviour
	{
		[SerializeField]
		private BlocsContainer blocsContainer;
		
		[SerializeField]
		private SpriteRenderer animationSpriteRenderer;
		
		private Coroutine animationCoroutine;
		
		public int rendererSortingOrder => animationSpriteRenderer.sortingOrder;

		public void SetRendererSortingOder(int sortingLayer)
			=> animationSpriteRenderer.sortingOrder = sortingLayer;

		public void PlayAnimation(IAnimationData animationData, AnimationFinishedDelegate finishedCallback = null)
		{
			if (animationCoroutine != null)
				StopCoroutine(animationCoroutine);
			
			animationCoroutine = StartCoroutine(AnimationsRoutine(animationData, finishedCallback));
		}

		private IEnumerator AnimationsRoutine(IAnimationData animationData, AnimationFinishedDelegate finishedCallback)
		{
			var currentAnimationFrameRate = animationData.GetAnimationFrameRate();
			
			var spriteIndex = 0;
			
			while (spriteIndex < animationData.animationSprites.Count)
			{
				animationSpriteRenderer.sprite = animationData.animationSprites[spriteIndex];
				
				for (var i = 0; i < currentAnimationFrameRate; i++)
					yield return null;
				
				spriteIndex++;

				if (animationData.isLoopAnimation && spriteIndex >= animationData.animationSprites.Count)
					spriteIndex = 0;
			}
			
			finishedCallback?.Invoke();
		}
	}
}