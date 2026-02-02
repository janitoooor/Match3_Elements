using System.Collections.Generic;
using Core.Level.Configs;
using UnityEngine;

namespace Core.Saves
{
	public interface IBlocksOnGridCoreSavesStorage
	{
		IReadOnlyDictionary<Vector2Int, ILevelBlockData> blockEntitiesDict { get; }
		void ClearLevelData();
	}
}