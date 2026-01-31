using Core.Enums;

namespace Core.Blocks
{
	public interface IBlocsContainer
	{
		IBlockSkinData GetBlockSkinData(BlockSkin blockSkin);
	}
}