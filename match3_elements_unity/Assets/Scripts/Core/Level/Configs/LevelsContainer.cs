using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using Core.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Level.Configs
{
	[Serializable]
	public sealed class LevelBlockData : ILevelBlockData
	{
		[field: SerializeField]
		public BlockSkin skin { get; private set; }

		[field: SerializeField]
		public Vector2Int cellPos { get; private set; }
	}
	
	[Serializable]
	public sealed class LevelData : ILevelData
	{
		[field: SerializeField]
		public byte minDestroyBlocksLineLength { get; private set; } = 3;
		
		[field: SerializeField]
		public int levelIndex { get; private set; }

		[field: SerializeField]
		public Vector2Int gridSize { get; private set; }
		
		[FormerlySerializedAs("gridCellBlockDataInternal")]
		[SerializeField, ArrayElementTitle("cellPos")]
		private LevelBlockData[] levelBlockDataInternal;
		
		public IReadOnlyList<LevelBlockData> levelBlockData => levelBlockDataInternal;
	}
	
	[CreateAssetMenu(menuName = "Match3/Core/Create Levels Container", order = 0, fileName = "LevelsContainer")]
	public sealed class LevelsContainer : ScriptableObject, ILevelsContainer
	{
		[SerializeField, ArrayElementTitle("levelIndex")]
		private LevelData[] levelsData;
		
		public ILevelData GetLevelData(int levelIndex)
			=> levelsData.FirstOrDefault(l => l.levelIndex == levelIndex);

		public int GetLevelsCount()
			=> levelsData.Length;
	}
}