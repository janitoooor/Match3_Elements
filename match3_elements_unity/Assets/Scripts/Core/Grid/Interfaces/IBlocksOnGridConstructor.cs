using Core.Blocks;
using Core.Level.Configs;

namespace Core.Grid
{
	public interface IBlocksOnGridConstructor
	{
		public void PlaceBlockOnGrid(IBlockEntity blockEntity, LevelBlockData blockData);
		public void ConstructGrid(int gridSizeX, int gridSizeY);
	}
}