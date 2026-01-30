using System.Collections;
using Core.Blocks;
using Core.Grid;
using Core.Level.Configs;
using Zenject;

namespace Core.Level
{
	public sealed class LevelConstructor : ILevelConstructor
	{
		private const byte MAX_SPAWNED_PER_FRAME = 10;
		
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
			blocksOnGridFieldProvider.ChangeGridSize(levelData.gridSize.x, levelData.gridSize.y);

			byte spawnedPerFrame = 0;
			
			for (var i = 0; i < levelData.levelBlockData.Count; i++)
			{
				var blockData = levelData.levelBlockData[i];
				blocksOnGridFieldProvider.AddBlockOnGrid(blocksGenerator.GenerateBlock(blockData.skin), blockData.cellPos);
				spawnedPerFrame++;

				if (spawnedPerFrame > MAX_SPAWNED_PER_FRAME)
				{
					spawnedPerFrame = 0;
					yield return null;
				}
			}
		}
	}
}