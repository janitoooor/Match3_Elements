using UnityEngine;

namespace Core.Grid
{
	public interface IGridField
	{
		float cellSize { get; }
		Vector2Int gridSize { get; }
		void UpdateGridSize(int x, int y);
		float GetGridWidth();
		float GetGridHeight();
		Vector2 GetGridCenter();
		Transform GetTransformParentForBlocksInCell();
	}
}