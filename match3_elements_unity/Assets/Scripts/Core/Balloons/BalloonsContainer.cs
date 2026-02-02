using System.Collections.Generic;
using UnityEngine;

namespace Core.Balloons
{
	[CreateAssetMenu(menuName = "Match3/Core/Create Balloons Container", order = 0, fileName = "BalloonsContainer")]
	public sealed class BalloonsContainer : ScriptableObject, IBalloonsContainer
	{
		[field: SerializeField]
		private Sprite[] balloonSpritesInternal;
		
		[field: SerializeField]
		public int maxBalloonsCount { get; private set; } = 3;
		
		[field: SerializeField, Range(0, 10f)]
		public float maxSpeed { get; private set; } = 3;
		
		[field: SerializeField, Range(0, 10f)]
		public float minSpeed { get; private set; } = 1;
		
		[field: SerializeField, Range(0, 5f)]
		public float amplitude { get; private set; } = 0.5f;
		
		[field: SerializeField, Range(0, 1.5f)]
		public float frequency { get; private set; } = 1f; 
		
		[field: SerializeField]
		public AnimationCurve waveCurve { get; private set; } = AnimationCurve.EaseInOut(0, 0, 1, 1);

		[field: SerializeField]
		public float spawnOffsetX { get; private set; } = 1f;

		[field: SerializeField]
		public float spawnOffsetYTop { get; private set; } = 1f;
		
		[field: SerializeField]
		public float spawnDivideOffsetY { get; private set; } = 4f;

		[field: SerializeField]
		public float spawnBalloonsCdMin { get; private set; } = 0.3f;
		
		[field: SerializeField]
		public float spawnBalloonsCdMax { get; private set; } = 0.6f;

		public IReadOnlyList<Sprite> balloonSprites => balloonSpritesInternal;
	}
}