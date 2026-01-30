using Core.Enums;

namespace Core.Blocks
{
	public interface IBlocksGenerator
	{
		IBlockEntity GenerateBlock(AnimationSkin skin);
	}
}