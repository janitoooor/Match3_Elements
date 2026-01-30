using Core.Animation.Configs;
using Core.Grid;
using UnityEngine;

namespace Core.Blocks
{
	public interface IBlockEntity
	{
		void MoveTo(Vector3 localPosition, MovedBlockDelegate callback);
		void Setup(IAnimationSkinData getAnimationSkinData);
		void PlaceAt(Transform transformParent, Vector3 localPosition);
		void KillBlock();
	}
}