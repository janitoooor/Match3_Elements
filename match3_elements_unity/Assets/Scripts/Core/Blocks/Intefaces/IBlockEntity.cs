using Core.Enums;
using UnityEngine;

namespace Core.Blocks
{
	public delegate void KilledBlockDelegate();
	
	public interface IBlockEntity
	{
		BlockSkin blockSkin { get; }
		float moveDuration { get; }
		int rendererSortingOrder { get; }
		void SetRendererSortingOder(int sortingLayer);
		void Setup(IBlockSkinData getBlockSkinData);
		void PlaceAt(Transform transformParent, Vector3 localPosition);
		void ShowBlock();
		void KillBlock(KilledBlockDelegate killedCallback);
		Vector3 GetLocalPosition();
		void SetLocalPosition(Vector3 localPosition);
	}
}