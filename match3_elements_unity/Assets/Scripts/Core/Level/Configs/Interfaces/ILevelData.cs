using System.Collections.Generic;
using Core.Level.Configs;
using UnityEngine;

namespace Core.Level.Configs
{
	public interface ILevelData
	{
		byte minDestroyBlocksLineLength { get; }
		Vector2Int gridSize { get; }
		IReadOnlyList<LevelBlockData> levelBlockData { get; }
	}
}