using System.Collections;
using System.Collections.Generic;
using Core.Blocks;
using Core.Grid;
using Core.Level.Configs;
using Core.Saves;
using UnityEngine;
using Zenject;

namespace Core.Level
{
	public sealed class LevelConstructor : ILevelConstructor
	{
		private const byte MAX_INSTANTIATED_BLOCS_PER_FRAME = 10;

		private readonly IBlocksOnGridCoreSavesStorage blocksOnGridCoreSavesStorage;
		private readonly IBlocksOnGridConstructor blocksOnGridConstructor;
		private readonly IBlocksGenerator blocksGenerator;

		[Inject]
		public LevelConstructor(
			IBlocksOnGridCoreSavesStorage blocksOnGridCoreSavesStorage,
			IBlocksOnGridConstructor blocksOnGridConstructor, 
			IBlocksGenerator blocksGenerator)
		{
			this.blocksOnGridCoreSavesStorage = blocksOnGridCoreSavesStorage;
			this.blocksOnGridConstructor = blocksOnGridConstructor;
			this.blocksGenerator = blocksGenerator;
		}

		public IEnumerator ConstructLevel(ILevelData levelData)
		{
			blocksOnGridConstructor.ConstructGrid(levelData.gridSize.x, levelData.gridSize.y);

			byte instantiatedBlocsPerFrame = 0;

			var blocsFromSaves = blocksOnGridCoreSavesStorage.blockEntitiesDict;

			if (blocsFromSaves.Count > 0)
				yield return GenerateLevelBySaves(blocsFromSaves, instantiatedBlocsPerFrame);
			else
				yield return GenerateLevelByConfig(levelData, instantiatedBlocsPerFrame);
		}

		private IEnumerator GenerateLevelBySaves(IReadOnlyDictionary<Vector2Int, ILevelBlockData> blocsFromSaves, 
			byte instantiatedBlocsPerFrame)
		{
			foreach (var block in blocsFromSaves)
			{
				if (GenerateBlockAndSkipFrameCheck(block.Value, ref instantiatedBlocsPerFrame)) 
					yield return null;
			}
		}
		
		private IEnumerator GenerateLevelByConfig(ILevelData levelData, byte instantiatedBlocsPerFrame)
		{
			for (var i = 0; i < levelData.levelBlockData.Count; i++)
				if (GenerateBlockAndSkipFrameCheck(levelData.levelBlockData[i], ref instantiatedBlocsPerFrame)) 
					yield return null;
		}

		private bool GenerateBlockAndSkipFrameCheck(ILevelBlockData blockData, ref byte instantiatedBlocsPerFrame)
		{
			var blockEntity = blocksGenerator.GenerateBlock(blockData.skin, out var isInstantiated);
				
			blocksOnGridConstructor.PlaceBlockOnGrid(blockEntity, blockData);

			if (isInstantiated)
				instantiatedBlocsPerFrame++;
			
			if (instantiatedBlocsPerFrame > MAX_INSTANTIATED_BLOCS_PER_FRAME)
			{
				instantiatedBlocsPerFrame = 0;
				return true;
			}
			
			return false;
		}
	}
}