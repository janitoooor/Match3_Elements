using System;
using System.Collections.Generic;
using Base;
using Core.Animation;
using Core.Enums;
using UnityEngine;

namespace Core.Blocks
{
	[Serializable]
	public sealed class AnimationData : IAnimationData
	{
		private	const float DEFAULT_ANIMATION_FRAME_RATE = 60;
		
		[field: SerializeField]
		public AnimationType animationType { get; private set; }
		
		[SerializeField]
		private int animationFrameRate = 2;

		[field: SerializeField]
		public bool isLoopAnimation { get; private set; }

		[SerializeField]
		private Sprite[] animationSpritesInternal;
		
		public IReadOnlyList<Sprite> animationSprites => animationSpritesInternal;
		
		private int fixedAnimationFrameRate = -1;
		
		public int GetAnimationFrameRate()
		{
			TrySetFixedAnimationFrameRate();

			return fixedAnimationFrameRate;
		}

		private void TrySetFixedAnimationFrameRate()
		{
			if (fixedAnimationFrameRate == -1)
				fixedAnimationFrameRate = Mathf.Max((int)(CalculateFixedAnimationFrameRate() + 0.5f), 1);
		}

		private float CalculateFixedAnimationFrameRate()
			=> animationFrameRate * Application.targetFrameRate / DEFAULT_ANIMATION_FRAME_RATE;
	}

	[Serializable]
	public sealed class BlockSkinData : IBlockSkinData
	{
		[field: SerializeField]
		public BlockSkin blockSkin { get; private set; }
    
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

	[CreateAssetMenu(menuName = "Match3/Core/Create Blocs Container", order = 0, fileName = "BlocsContainer")]
	public sealed class BlocsContainer : ScriptableObject, IBlocsContainer
	{
		[SerializeField, ArrayElementTitle("blockSkin")]
		private BlockSkinData[] blockSkinsData;

		private readonly Dictionary<BlockSkin, IBlockSkinData> cache = new();
    
		public IBlockSkinData GetBlockSkinData(BlockSkin blockSkin)
			=> cache.GetOrAddFromArray(
				blockSkin,
				blockSkinsData,
				a => a.blockSkin,
				a => a);
	}
}