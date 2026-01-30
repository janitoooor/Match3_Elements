using System;
using Core.Blocks;
using UnityEngine;

namespace Core.Grid
{
	public sealed class GridField : MonoBehaviour, IGridField, IGridFieldInfo
	{
		[SerializeField]
		private float minPaddingX = 1f;
		
		[SerializeField]
		private float minPaddingY = 1f;
		
		[SerializeField]
		private Vector2Int gridDimensions = new(10, 10);
		
		[SerializeField]
		private float cellSize = 1f;
		
#if UNITY_EDITOR
		[SerializeField]
		private Color gridColor = Color.white;
		
		private void OnDrawGizmos()
		{
			if (!enabled) 
				return;
			
			Gizmos.color = gridColor;
			var startPos = transform.position;

			for (var x = 0; x <= gridDimensions.x; x++)
			{
				var start = startPos + new Vector3(x * cellSize, 0, 0);
				var end = startPos + new Vector3(x * cellSize, gridDimensions.y * cellSize, 0);
				Gizmos.DrawLine(start, end);
			}

			for (var y = 0; y <= gridDimensions.y; y++)
			{
				var start = startPos + new Vector3(0, y * cellSize, 0);
				var end = startPos + new Vector3(gridDimensions.x * cellSize, y * cellSize, 0);
				Gizmos.DrawLine(start, end);
			}

			var center = GetGridCenter();
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(center, 0.1f);
		}
#endif
		
		public void PrepareGridSize(int x, int y)
			=> gridDimensions = new Vector2Int(x, y);

		public void PlaceBlockAtCell(IBlockEntity blockEntity, Vector2Int cell)
		{
			if (cell.x < 0 || cell.x >= gridDimensions.x || cell.y < 0 || cell.y >= gridDimensions.y)
				throw new ArgumentOutOfRangeException();

			blockEntity.PlaceAt(transform, CalculateBlockCellPosition(cell.x, cell.y));
		}

		public float GetGridWidth()
			=> gridDimensions.x * cellSize + minPaddingX;

		public float GetGridHeight()
			=> gridDimensions.y * cellSize + minPaddingY;
		
		public Vector2 GetGridCenter()
		{
			var centerX = transform.position.x + gridDimensions.x * cellSize / 2;
			var centerY = transform.position.y + gridDimensions.y * cellSize / 2;
			
			return new Vector2(centerX, centerY);
		}
		
		private Vector3 CalculateBlockCellPosition(int x, int y)
		{
			return new Vector3(
				CalculateCellSizeOffset(x), 
				CalculateCellSizeOffset(y),
				CalculateZOffset(x, y));
		}

		private static float CalculateZOffset(int x, int y)
			=> -(y * 0.1f + x * 0.05f);

		private float CalculateCellSizeOffset(int y)
			=> y * cellSize + cellSize / 2;
	}
}