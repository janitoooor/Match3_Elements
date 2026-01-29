using System.Collections.Generic;
using UnityEngine;

namespace Core.Animation.Configs
{
	public interface IAnimationData
	{
		int animationFrameRate { get; }
		bool isLoopAnimation { get; }
		IReadOnlyList<Sprite> animationSprites { get; }
	}
}