using Core.Enums;

namespace Core.Animation.Configs
{
	public interface IAnimationSkinData
	{
		IAnimationData GetAnimationData(AnimationType animationType);
	}
}