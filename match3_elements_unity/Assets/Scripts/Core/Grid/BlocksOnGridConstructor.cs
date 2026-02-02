using Core.Blocks;
using Core.Level.Configs;
using Zenject;

namespace Core.Grid
{
	public sealed class BlocksOnGridConstructor : IBlocksOnGridConstructor
	{
		private readonly IBlockOnGridRendererSortingOderProvider blockOnGridRendererSortingOderProvider;
		private readonly IBlocksOnGridRepository blocksOnGridRepository;

		[Inject]
		public BlocksOnGridConstructor(
			IBlockOnGridRendererSortingOderProvider blockOnGridRendererSortingOderProvider,
			IBlocksOnGridRepository blocksOnGridRepository)
		{
			this.blockOnGridRendererSortingOderProvider = blockOnGridRendererSortingOderProvider;
			this.blocksOnGridRepository = blocksOnGridRepository;
		}

		public void PlaceBlockOnGrid(IBlockEntity blockEntity, LevelBlockData blockData)
		{
			blocksOnGridRepository.AddBlockOnGrid(blockEntity, blockData.cellPos);

			blockOnGridRendererSortingOderProvider.SetBlockRendererSortingOderForCell(blockEntity, blockData.cellPos);
		}

		public void ConstructGrid(int gridSizeX, int gridSizeY)
			=> blocksOnGridRepository.SetGridSize(gridSizeX, gridSizeY);
	}
}