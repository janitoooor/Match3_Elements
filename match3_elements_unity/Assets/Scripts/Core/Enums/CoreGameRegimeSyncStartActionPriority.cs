namespace Core.Enums
{
	public enum CoreGameRegimeSyncStartActionPriority : byte
	{
		MainWidget,
		CameraGridFieldFit,
		NewLevelBlocksOnGridShow,
		BalloonActivate,
		NewLevelBlocksOnGridFall,
		NewLevelBlocksOnGridKill,
		
		SwipeInputInitialize = byte.MaxValue,
	}
}