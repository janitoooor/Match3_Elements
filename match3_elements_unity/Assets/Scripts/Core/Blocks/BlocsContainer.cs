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