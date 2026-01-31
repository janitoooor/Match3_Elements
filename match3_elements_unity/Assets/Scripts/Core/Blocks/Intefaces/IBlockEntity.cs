using UnityEngine;

namespace Core.Blocks
{
	public interface IBlockEntity
	{
		float moveDuration { get; }
		int rendererSortingOrder { get; }
		void SetRendererSortingOder(int sortingLayer);
		void Setup(IBlockSkinData getBlockSkinData);
		void PlaceAt(Transform transformParent, Vector3 localPosition);
		void ShowBlock();
		void KillBlock();
		Vector3 GetLocalPosition();
		void SetLocalPosition(Vector3 localPosition);
	}
}