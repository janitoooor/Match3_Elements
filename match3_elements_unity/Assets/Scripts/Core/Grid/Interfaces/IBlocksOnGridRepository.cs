using System.Collections.Generic;
using Core.Blocks;
using Core.Input;
using UnityEngine;

namespace Core.Grid
{
	public delegate void OnBlocksOnGridChangedDelegate(Vector2Int cellPos, IBlockEntity blockEntity);
	
	public interface IBlocksOnGridRepository
	{
		event OnBlocksOnGridChangedDelegate OnBlockOnGridChanged;
		
		IReadOnlyDictionary<IBlockEntity, Vector2Int> blocksOnGridField { get; }
		IReadOnlyDictionary<Vector2Int, IBlockEntity> gridCells { get; }
		IReadOnlyCollection<IBlockEntity> killedBlocks { get; }
		Vector2Int gridSize { get; }
		Dictionary<IBlockEntity, Vector2Int> GetSaveBlocksOnGridField();
		void SetCellBusy(Vector2Int blockCell);
		void SetCellUnBusy(Vector2Int blockCell);
		void SetCellBusyByKill(Vector2Int blockCell);
		void SetCellUnBusyByKill(Vector2Int blockCell);
		void RemoveBlockFromGrid(IBlockEntity blockEntity, Vector2Int blockCell);
		void SetBlockOnGrid(IBlockEntity blockEntity, Vector2Int cell);
		void RemoveBlockFromCell(Vector2Int sourceCell);
		void AddKilledBlock(IBlockEntity blockEntity);
		void RemoveKilledBlocks(IBlockEntity blockEntity);
		void AddBlockOnGrid(IBlockEntity blockEntity, Vector2Int cell);
		void SetGridSize(int x, int y);
		Vector2Int GetBlockCellToSwipe(IBlockEntity blockEntity, SwipeDirectionData swipeDirectionData);
		Vector3 CalculateBlockLocalPositionOnGridCell(Vector2Int cell);
		bool IsCellBusy(Vector2Int cell);
	}
}