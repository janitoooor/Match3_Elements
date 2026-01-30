using System.Collections.Generic;
using System.Linq;
using Core.Blocks;
using UnityEngine;

namespace Core.SpawnContainer
{
	public sealed class SpawnerInContainer : MonoBehaviour, ISpawnerInContainer
	{
		[SerializeField]
		private SpawnContainer[] spawnContainers;
		
		private readonly Dictionary<SpawnContainerType, SpawnContainer> spawnContainersCache = new();
		
		public T SpawnInContainer<T>(T prefab, SpawnContainerType containerType) where T : Object
			=> Instantiate(prefab, GetSpawnContainer(containerType));

		public IReadOnlyList<T> GetContainerSpawnedEntities<T>(SpawnContainerType containerType)
			=> GetSpawnContainer(containerType).GetComponentsInChildren<T>(true);

		private Transform GetSpawnContainer(SpawnContainerType containerType)
		{
			if (spawnContainersCache.TryGetValue(containerType, out var container))
				return container.transform;
			
			container = spawnContainers.First(c => c.containerType == containerType);
			spawnContainersCache.Add(containerType, container);
			return container.transform;
		}
	}
}