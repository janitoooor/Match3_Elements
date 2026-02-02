using Core.Animation;
using Core.Enums;

namespace Core.Blocks
{
	public interface IBlockSkinData
	{
		BlockSkin blockSkin { get; }
		IAnimationData GetAnimationData(AnimationType animationType);
	}
}