using Core.Enums;
using UnityEngine;

namespace Core.Level.Configs
{
	public interface ILevelBlockData
	{
		AnimationSkin skin { get; }
		Vector2Int cellPos { get; }
	}
}