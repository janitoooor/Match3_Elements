namespace Core.Enums
{
	public enum CoreGameRegimeSyncStartActionPriority : byte
	{
		MainWidget,
		CameraGridFieldFit,
		NewLevelBlocksOnGridShow,
		BalloonActivate,
		
		SwipeInputInitialize = byte.MaxValue,
	}
}