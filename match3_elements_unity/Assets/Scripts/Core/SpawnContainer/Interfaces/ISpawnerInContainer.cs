using System.Collections.Generic;
using Core.Blocks;

namespace Core.SpawnContainer
{
	public interface ISpawnerInContainer
	{
		IReadOnlyList<T> GetContainerSpawnedEntities<T>(SpawnContainerType containerType);
		T SpawnInContainer<T>(T prefab, SpawnContainerType containerType) where T : UnityEngine.Object;
	}
}