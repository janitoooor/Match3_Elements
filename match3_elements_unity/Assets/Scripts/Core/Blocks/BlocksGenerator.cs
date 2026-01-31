using System.Collections.Generic;
using Core.Enums;
using Core.Interfaces;
using Core.SpawnContainer;
using Zenject;

namespace Core.Blocks
{
	public sealed class BlocksGenerator : IBlocksGenerator
	{
		private const SpawnContainerType SPAWN_CONTAINER_TYPE =  SpawnContainerType.Blocks;
		
		private readonly ISpawnerInContainer spawnerInContainer;
		private readonly ICorePrefabsContainer corePrefabsContainer;
		private readonly IBlocsContainer blocsContainer;

		private readonly List<IBlockEntity> spawnedBlocks;
		private readonly Queue<IBlockEntity> blocksPool;
		
		[Inject]
		public BlocksGenerator(
			ISpawnerInContainer spawnerInContainer, 
			ICorePrefabsContainer corePrefabsContainer,
			IBlocsContainer blocsContainer)
		{
			this.spawnerInContainer = spawnerInContainer;
			this.corePrefabsContainer = corePrefabsContainer;
			this.blocsContainer = blocsContainer;

			spawnedBlocks = new(GetContainerSpawnedBlocks(spawnerInContainer));
			blocksPool = new (spawnedBlocks);
		}

		private static IReadOnlyList<IBlockEntity> GetContainerSpawnedBlocks(ISpawnerInContainer spawnerInContainer)
			=> spawnerInContainer.GetContainerSpawnedEntities<IBlockEntity>(SPAWN_CONTAINER_TYPE);

		public IBlockEntity GenerateBlock(BlockSkin skin, out bool isInstantiated)
		{
			var block = GetBlockEntity(out isInstantiated);
			block.Setup(blocsContainer.GetBlockSkinData(skin));
			return block;
		}

		private IBlockEntity GetBlockEntity(out bool isInstantiated)
		{
			isInstantiated = false;

			if (blocksPool.Count > 0)
				blocksPool.Dequeue();

			isInstantiated = true;
			return CreateNewBlockEntity();
		}

		private IBlockEntity CreateNewBlockEntity()
		{
			var block = spawnerInContainer.SpawnInContainer(GetBlockPrefab(), SPAWN_CONTAINER_TYPE);
			spawnedBlocks.Add(block);
			block.OnBlockEntityDead += BlockOnOnBlockEntityDead;
			
			return block;
		}

		private void BlockOnOnBlockEntityDead(IBlockEntity blockEntity)
			=> blocksPool.Enqueue(blockEntity);

		private BlockEntity GetBlockPrefab()
			=> corePrefabsContainer.GetPrefab<BlockEntity>(CorePrefabsKeys.BlockEntity);
	}
}