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
		private readonly Queue<IBlockEntity> blocksPool = new();
		
		[Inject]
		public BlocksGenerator(
			ISpawnerInContainer spawnerInContainer, 
			ICorePrefabsContainer corePrefabsContainer,
			IBlocsContainer blocsContainer)
		{
			this.spawnerInContainer = spawnerInContainer;
			this.corePrefabsContainer = corePrefabsContainer;
			this.blocsContainer = blocsContainer;

			spawnedBlocks = new(spawnerInContainer.GetContainerSpawnedEntities<BlockEntity>(SPAWN_CONTAINER_TYPE));
			blocksPool = new (spawnedBlocks);
		}

		public IBlockEntity GenerateBlock(BlockSkin skin)
		{
			var block = GetBlockEntity();
			block.Setup(blocsContainer.GetBlockSkinData(skin));
			return block;
		}

		private IBlockEntity GetBlockEntity()
			=> blocksPool.Count == 0 ? CreateNewBlockEntity() : blocksPool.Dequeue();

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