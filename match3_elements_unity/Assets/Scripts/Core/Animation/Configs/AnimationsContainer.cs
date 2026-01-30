using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using Core.Enums;
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
    
		private readonly Dictionary<AnimationType, IAnimationData> cache = new();
    
		public IAnimationData GetAnimationData(AnimationType animationType)
			=> cache.GetOrAddFromArray(
				animationType,
				animationsData,
				a => a.animationType,
				a => a
			);
	}

	[CreateAssetMenu(menuName = "Match3/Core/Create Animations Container", order = 0, fileName = "AnimationsContainer")]
	public sealed class AnimationsContainer : ScriptableObject, IAnimationsContainer
	{
		[SerializeField, ArrayElementTitle("animationSkin")]
		private AnimationSkinData[] animations;

		private readonly Dictionary<AnimationSkin, IAnimationSkinData> cache = new();
    
		public IAnimationSkinData GetAnimationSkinData(AnimationSkin animationSkin)
			=> cache.GetOrAddFromArray(
				animationSkin,
				animations,
				a => a.animationSkin,
				a => a);
	}
}