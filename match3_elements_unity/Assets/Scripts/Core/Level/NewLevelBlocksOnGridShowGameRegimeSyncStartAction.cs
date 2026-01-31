using Base;
using Core.Enums;
using Core.Grid;
using Zenject;

namespace Core.Level
{
	public sealed class NewLevelBlocksOnGridShowGameRegimeSyncStartAction : GameRegimeSyncStartAction
	{
		private readonly IBlocksOnGridFieldProvider blocksOnGridFieldProvider;
		public override byte priority => (int)CoreGameRegimeSyncStartActionPriority.NewLevelBlocksOnGridShow;

		[Inject]
		public NewLevelBlocksOnGridShowGameRegimeSyncStartAction(IBlocksOnGridFieldProvider blocksOnGridFieldProvider)
			=> this.blocksOnGridFieldProvider = blocksOnGridFieldProvider;

		public override void Perform()
		{
			foreach(var blockOnGridField in blocksOnGridFieldProvider.blocksOnGridField)
				blockOnGridField.Key.ShowBlock();
		}
	}
}