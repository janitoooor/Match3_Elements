using Base;
using Core.Saves;
using Zenject;

namespace Core.Level
{
	public sealed class LevelRestarter : ILevelRestarter
	{
		private readonly IGameRegimeLoader gameRegimeLoader;
		private readonly IBlocksOnGridCoreSavesStorage blocksOnGrid;

		[Inject]
		public LevelRestarter(IGameRegimeLoader gameRegimeLoader, IBlocksOnGridCoreSavesStorage blocksOnGrid)
		{
			this.gameRegimeLoader = gameRegimeLoader;
			this.blocksOnGrid = blocksOnGrid;
		}

		public void RestartLevel()
		{
			blocksOnGrid.ClearLevelData();
			gameRegimeLoader.RestartCurrentGameRegime();
		}
	}
}