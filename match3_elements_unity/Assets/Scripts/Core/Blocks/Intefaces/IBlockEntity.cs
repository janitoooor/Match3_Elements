using Core.Animation.Configs;
using UnityEngine;

namespace Core.Blocks
{
	public interface IBlockEntity
	{
		void Setup(IAnimationSkinData getAnimationSkinData);
		void PlaceAt(Transform transformParent, Vector3 localPosition);
		void KillBlock();
	}
}