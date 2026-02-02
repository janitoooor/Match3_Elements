using System.Collections.Generic;
using UnityEngine;

namespace Core.Balloons
{
	public interface IBalloonsContainer
	{
		int maxBalloonsCount { get; }
		float maxSpeed { get; }
		float minSpeed { get; }
		float amplitude { get; }
		float frequency { get; }
		IReadOnlyList<Sprite> balloonSprites { get; }
		AnimationCurve waveCurve { get; }
		float spawnOffsetX { get; }
		float spawnOffsetYTop { get; }
		float spawnDivideOffsetY { get; }
		float spawnBalloonsCdMin { get; }
		float spawnBalloonsCdMax { get; }
	}
}