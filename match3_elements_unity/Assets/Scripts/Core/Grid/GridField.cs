using UnityEngine;

namespace Core.Grid
{
	public sealed class GridField : MonoBehaviour, IGridField
	{
		[SerializeField]
		private float minPaddingX = 1f;
		
		[SerializeField]
		private float minPaddingY = 1f;
		
		[field: SerializeField]
		public Vector2Int gridSize { get; private set; } = new (10, 10);
		
		[field: SerializeField]
		public float cellSize { get; private set; } = 1f;
		
#if UNITY_EDITOR
		[SerializeField]
		private Color gridColor = Color.white;

		private void OnDrawGizmos()
		{
			if (!enabled) 
				return;
			
			Gizmos.color = gridColor;
			var startPos = transform.position;

			for (var x = 0; x <= gridSize.x; x++)
			{
				var start = startPos + new Vector3(x * cellSize, 0, 0);
				var end = startPos + new Vector3(x * cellSize, gridSize.y * cellSize, 0);
				Gizmos.DrawLine(start, end);
			}

			for (var y = 0; y <= gridSize.y; y++)
			{
				var start = startPos + new Vector3(0, y * cellSize, 0);
				var end = startPos + new Vector3(gridSize.x * cellSize, y * cellSize, 0);
				Gizmos.DrawLine(start, end);
			}

			var center = GetGridCenter();
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(center, 0.1f);
		}
#endif
		
		public void UpdateGridSize(int x, int y)
			=> gridSize = new Vector2Int(x, y);
		
		public float GetGridWidth()
			=> gridSize.x * cellSize + minPaddingX;

		public float GetGridHeight()
			=> gridSize.y * cellSize + minPaddingY;
		
		public Vector2 GetGridCenter()
		{
			var centerX = transform.position.x + gridSize.x * cellSize / 2;
			var centerY = transform.position.y + gridSize.y * cellSize / 2;
			
			return new Vector2(centerX, centerY);
		}

		public Transform GetTransformParentForBlocksInCell()
			=> transform;
	}
}