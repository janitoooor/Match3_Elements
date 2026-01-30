using Core.Enums;
using UnityEngine;

namespace Core.Input
{
	public struct SwipeDirectionData
	{
		public SwipeDirectionType directionType { get; }
		public Vector2Int directionVector { get; }

		public SwipeDirectionData(SwipeDirectionType directionType, Vector2Int directionVector)
		{
			this.directionType = directionType;
			this.directionVector = directionVector;
		}
	}
}