using System.Collections.Generic;
using Core.Blocks;
using Core.Enums;
using Core.Interfaces;
using Core.SpawnContainer;
using Zenject;

namespace Core.Balloons
{
	public sealed class BalloonGenerator : IBalloonGenerator
	{
		private const SpawnContainerType SPAWN_CONTAINER_TYPE = SpawnContainerType.Balloon;
		
		private readonly ISpawnerInContainer spawnerInContainer;
		private readonly ICorePrefabsContainer corePrefabsContainer;

		private readonly List<IBalloonEntity> spawnedBalloons;
		private readonly Queue<IBalloonEntity> pool;
		
		[Inject]
		public BalloonGenerator(
			ISpawnerInContainer spawnerInContainer, 
			ICorePrefabsContainer corePrefabsContainer)
		{
			this.spawnerInContainer = spawnerInContainer;
			this.corePrefabsContainer = corePrefabsContainer;

			spawnedBalloons = new(GetContainerSpawnedBalloons(spawnerInContainer));
			pool = new (spawnedBalloons);
		}

		private static IReadOnlyList<IBalloonEntity> GetContainerSpawnedBalloons(ISpawnerInContainer spawnerInContainer)
			=> spawnerInContainer.GetContainerSpawnedEntities<IBalloonEntity>(SPAWN_CONTAINER_TYPE);

		public IBalloonEntity GenerateBalloon(out bool isInstantiated)
		{
			var balloon = GetBalloonEntity(out isInstantiated);
			return balloon;
		}

		private IBalloonEntity GetBalloonEntity(out bool isInstantiated)
		{
			isInstantiated = false;

			if (pool.Count > 0)
				pool.Dequeue();

			isInstantiated = true;
			return CreateNewBalloonEntity();
		}

		private IBalloonEntity CreateNewBalloonEntity()
		{
			var balloon = spawnerInContainer.SpawnInContainer(GetPrefab(), SPAWN_CONTAINER_TYPE);
			spawnedBalloons.Add(balloon);
			
			return balloon;
		}

		private BalloonEntity GetPrefab()
			=> corePrefabsContainer.GetPrefab<BalloonEntity>(CorePrefabsKeys.BalloonEntity);
	}
}