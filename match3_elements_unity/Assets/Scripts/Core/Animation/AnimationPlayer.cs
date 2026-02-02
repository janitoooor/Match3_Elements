using System.Collections;
using Core.Blocks;
using UnityEngine;

namespace Core.Animation
{
	public delegate void AnimationFinishedDelegate();
	
	public sealed class AnimationPlayer : MonoBehaviour
	{
		private	const float DEFAULT_FRAME_RATE = 60;
		
		[SerializeField]
		private BlocsContainer blocsContainer;
		
		[SerializeField]
		private SpriteRenderer animationSpriteRenderer;
		
		private Coroutine animationCoroutine;

		private int animationFrameRate = (int)DEFAULT_FRAME_RATE;

		public int rendererSortingOrder => animationSpriteRenderer.sortingOrder;

		public void SetRendererSortingOder(int sortingLayer)
			=> animationSpriteRenderer.sortingOrder = sortingLayer;

		public void PlayAnimation(IAnimationData animationData, AnimationFinishedDelegate finishedCallback = null)
		{
			if (animationCoroutine != null)
				StopCoroutine(animationCoroutine);
				
			animationFrameRate = (int)(GetAnimationDataFrameRate(animationData) + 0.5f);
			
			animationCoroutine = StartCoroutine(AnimationsRoutine(animationData, finishedCallback));
		}

		private IEnumerator AnimationsRoutine(IAnimationData animationData, AnimationFinishedDelegate finishedCallback)
		{
			var spriteIndex = 0;
			
			while (spriteIndex < animationData.animationSprites.Count)
			{
				animationSpriteRenderer.sprite = animationData.animationSprites[spriteIndex];
				
				for (var i = 0; i < animationFrameRate; i++)
					yield return null;
				
				spriteIndex++;

				if (animationData.isLoopAnimation && spriteIndex >= animationData.animationSprites.Count)
					spriteIndex = 0;
			}
			
			finishedCallback?.Invoke();
		}

		private static float GetAnimationDataFrameRate(IAnimationData animationData)
			=> animationData.animationFrameRate * Application.targetFrameRate / DEFAULT_FRAME_RATE;
	}
}