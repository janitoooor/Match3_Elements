using Base;
using Core.Enums;
using Core.Grid;
using Zenject;

namespace Core.Level
{
	public sealed class NewLevelBlocksOnGridShowGameRegimeSyncStartAction : GameRegimeSyncStartAction
	{
		private readonly IBlocksOnGridRepository blocksOnGridRepository;
		public override byte priority => (int)CoreGameRegimeSyncStartActionPriority.NewLevelBlocksOnGridShow;

		[Inject]
		public NewLevelBlocksOnGridShowGameRegimeSyncStartAction(IBlocksOnGridRepository blocksOnGridRepository)
			=> this.blocksOnGridRepository = blocksOnGridRepository;

		public override void Perform()
		{
			foreach(var blockOnGridField in blocksOnGridRepository.blocksOnGridField)
				blockOnGridField.Key.ShowBlock();
		}
	}
}