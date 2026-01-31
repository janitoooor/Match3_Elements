using Core.Enums;

namespace Core.Blocks
{
	public interface IBlocksGenerator
	{
		IBlockEntity GenerateBlock(BlockSkin skin, out bool isInstantiated);
	}
}