using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using Core.Animation.Enums;
using UnityEngine;

namespace Core.Animation.Configs
{
	[Serializable]
	public sealed class AnimationData : IAnimationData
	{
		[field: SerializeField]
		public AnimationType animationType { get; private set; }
		
		[field:SerializeField]
		public int animationFrameRate { get; private set; } = 2;

		[field: SerializeField]
		public bool isLoopAnimation { get; private set; }

		[SerializeField]
		private Sprite[] animationSpritesInternal;
		
		public IReadOnlyList<Sprite> animationSprites => animationSpritesInternal;
	}
	
	[Serializable]
	public sealed class AnimationSkinData : IAnimationSkinData
	{
		[field: SerializeField]
		public AnimationSkin animationSkin { get; private set; }
		
		[SerializeField, ArrayElementTitle("animationType")]
		private AnimationData[] animationsData;
		
		public AnimationData GetAnimationData(AnimationType animationType)
			=> animationsData.FirstOrDefault(a => a.animationType == animationType);
	}
	
	[CreateAssetMenu(menuName = "Match3/Core/Create Animations Container", order = 0, fileName = "AnimationsContainer")]
	public sealed class AnimationsContainer : ScriptableObject, IAnimationsContainer
	{
		[SerializeField, ArrayElementTitle("animationSkin")]
		private AnimationSkinData[] animations;

		public IAnimationSkinData GetAnimationSkinData(AnimationSkin animationSkin)
			=> animations.FirstOrDefault(a => a.animationSkin == animationSkin);
	}
}