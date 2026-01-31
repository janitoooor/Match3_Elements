using Core.Animation;
using Core.Enums;

namespace Core.Blocks
{
	public interface IBlockSkinData
	{
		IAnimationData GetAnimationData(AnimationType animationType);
	}
}