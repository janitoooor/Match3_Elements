using UnityEngine;

namespace Core.Grid
{
	public interface IGridFieldInfo
	{
		Vector2Int gridSize { get; }
		float GetGridWidth();
		float GetGridHeight();
		Vector2 GetGridCenter();
	}
}