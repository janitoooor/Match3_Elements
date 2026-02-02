using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base;
using Common.Saves;
using Core.Blocks;
using Core.Enums;
using Core.Grid;
using Core.Level.Configs;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Core.Saves
{
	public sealed class BlocksOnGridCoreSavesStorage : AsyncDataInitializer, IBlocksOnGridCoreSavesStorage
	{
		private const string SAVE_KEY = "BO_GC";
		public override byte priority => (byte)CoreAsyncDataInitializePriority.CoreSaves;
		
		private readonly ISavesWriter savesWriter;
		private readonly IBlocksOnGridRepository blocksOnGridRepository;
		
		private Dictionary<Vector2Int, ILevelBlockData> blockEntitiesDictInternal;

		public IReadOnlyDictionary<Vector2Int, ILevelBlockData> blockEntitiesDict => 
			new Dictionary<Vector2Int, ILevelBlockData>(blockEntitiesDictInternal);

		[Inject]
		public BlocksOnGridCoreSavesStorage(ISavesWriter savesWriter, IBlocksOnGridRepository blocksOnGridRepository)
		{
			this.savesWriter = savesWriter;
			blocksOnGridRepository.OnBlockOnGridChanged += OnBlockOnGridChanged;
		}

		public override IEnumerator Initialize()
		{
			var blocksData = savesWriter.ReadSave<BlockOnGridData[]>(SAVE_KEY);
			
			blockEntitiesDictInternal = new Dictionary<Vector2Int, ILevelBlockData>(blocksData?.Length ?? 0);
			
			if (blocksData != null)
			{
				foreach (var blockData in blocksData)
					blockEntitiesDictInternal.Add(blockData.cellPos, blockData);
			}
			
			yield return null;
		}

		public void ClearLevelData()
			=> savesWriter.DeleteSave(SAVE_KEY);

		private void OnBlockOnGridChanged(Vector2Int cellPos, IBlockEntity blockEntity)
		{
			if (blockEntity == null)
				blockEntitiesDictInternal.Remove(cellPos);
			else
				UpdateBlockInCellSave(cellPos, blockEntity);
			
			UpdateSave();
		}

		private void UpdateBlockInCellSave(Vector2Int cellPos, IBlockEntity blockEntity)
		{
			var blockOnGridData = CreateBlockOnGridData(cellPos, blockEntity);

			if (!blockEntitiesDictInternal.TryAdd(cellPos, blockOnGridData))
				blockEntitiesDictInternal[cellPos] = blockOnGridData;
		}

		private static BlockOnGridData CreateBlockOnGridData(Vector2Int cellPos, IBlockEntity blockEntity)
			=> new(cellPos.x, cellPos.y, blockEntity.blockSkin);

		private void UpdateSave() 
			=> savesWriter.WriteSave(SAVE_KEY, blockEntitiesDict.Values.ToArray());
		
		private sealed class BlockOnGridData : ILevelBlockData
		{
			[JsonProperty("a")]
			private readonly int cellX;

			[JsonProperty("b")]
			private readonly int cellY;
			
			[JsonProperty("d")]
			public BlockSkin skin { get; }
			public Vector2Int cellPos => new(cellX, cellY);
			
			public BlockOnGridData(int cellX, int cellY, BlockSkin skin)
			{
				this.cellX = cellX;
				this.cellY = cellY;
				this.skin = skin;
			}
		}
	}
}