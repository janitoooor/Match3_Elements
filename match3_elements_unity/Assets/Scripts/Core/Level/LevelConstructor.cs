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
		
		private readonly IGridField gridField;
		private readonly IBlocksGenerator blocksGenerator;

		[Inject]
		public LevelConstructor(IGridField gridField, IBlocksGenerator blocksGenerator)
		{
			this.gridField = gridField;
			this.blocksGenerator = blocksGenerator;
		}

		public IEnumerator ConstructLevel(ILevelData levelData)
		{
			gridField.PrepareGridSize(levelData.gridSize.x, levelData.gridSize.y);

			byte spawnedPerFrame = 0;
			
			for (var i = 0; i < levelData.levelBlockData.Count; i++)
			{
				var blockData = levelData.levelBlockData[i];
				gridField.PlaceBlockAtCell(blocksGenerator.GenerateBlock(blockData.skin), blockData.cellPos);
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