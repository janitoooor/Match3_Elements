using System.Collections;
using Core.Blocks;
using Core.Grid;
using Core.Level.Configs;
using Zenject;

namespace Core.Level
{
	public sealed class LevelConstructor : ILevelConstructor
	{
		private const byte MAX_INSTANTIATED_BLOCS_PER_FRAME = 10;
		
		private readonly IBlocksOnGridFieldProvider blocksOnGridFieldProvider;
		private readonly IBlocksGenerator blocksGenerator;

		[Inject]
		public LevelConstructor(IBlocksOnGridFieldProvider blocksOnGridFieldProvider, IBlocksGenerator blocksGenerator)
		{
			this.blocksOnGridFieldProvider = blocksOnGridFieldProvider;
			this.blocksGenerator = blocksGenerator;
		}

		public IEnumerator ConstructLevel(ILevelData levelData)
		{
			blocksOnGridFieldProvider.SetGridSize(levelData.gridSize.x, levelData.gridSize.y);

			byte instantiatedBlocsPerFrame = 0;
			
			for (var i = 0; i < levelData.levelBlockData.Count; i++)
				if (GenerateBlockAndSkipFrameCheck(levelData, i, ref instantiatedBlocsPerFrame)) 
					yield return null;
		}

		private bool GenerateBlockAndSkipFrameCheck(ILevelData levelData, int i, ref byte instantiatedBlocsPerFrame)
		{
			var blockData = levelData.levelBlockData[i];
			var blockEntity = blocksGenerator.GenerateBlock(blockData.skin, out var isInstantiated);
				
			blocksOnGridFieldProvider.AddBlockOnGrid(blockEntity, blockData.cellPos);
				
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