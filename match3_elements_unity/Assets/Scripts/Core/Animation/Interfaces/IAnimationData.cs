using System.Collections.Generic;
using UnityEngine;

namespace Core.Animation
{
	public interface IAnimationData
	{
		bool isLoopAnimation { get; }
		IReadOnlyList<Sprite> animationSprites { get; }
		int GetAnimationFrameRate();
	}
}