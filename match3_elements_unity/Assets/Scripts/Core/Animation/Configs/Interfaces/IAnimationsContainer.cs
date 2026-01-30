using Core.Enums;

namespace Core.Animation.Configs
{
	public interface IAnimationsContainer
	{
		IAnimationSkinData GetAnimationSkinData(AnimationSkin animationSkin);
	}
}