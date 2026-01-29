using Core.Animation.Enums;

namespace Core.Animation.Configs
{
	public interface IAnimationSkinData
	{
		AnimationData GetAnimationData(AnimationType animationType);
	}
}