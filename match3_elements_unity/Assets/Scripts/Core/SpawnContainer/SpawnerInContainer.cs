using System.Collections.Generic;
using Base;
using Core.Blocks;
using UnityEngine;

namespace Core.SpawnContainer
{
	public sealed class SpawnerInContainer : MonoBehaviour, ISpawnerInContainer
	{
		[SerializeField]
		private SpawnContainer[] spawnContainers;
		
		private readonly Dictionary<SpawnContainerType, SpawnContainer> cache = new();
		
		public T SpawnInContainer<T>(T prefab, SpawnContainerType containerType) where T : Object
			=> Instantiate(prefab, GetSpawnContainer(containerType));

		public IReadOnlyList<T> GetContainerSpawnedEntities<T>(SpawnContainerType containerType)
			=> GetSpawnContainer(containerType).GetComponentsInChildren<T>(true);

		private Transform GetSpawnContainer(SpawnContainerType containerType)
			=> cache.GetOrAddFromArray(
				containerType,
				spawnContainers,
				a => a.containerType,
				a => a).transform;
	}
}