using UnityEngine;

namespace Core.Grid
{
	public interface IGridFieldInfo
	{
		float GetGridWidth();
		float GetGridHeight();
		Vector2 GetGridCenter();
	}
}