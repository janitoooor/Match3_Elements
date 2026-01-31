namespace Core.Enums
{
	public enum CoreGameRegimeSyncStartActionPriority : byte
	{
		MainWidget,
		CameraGridFieldFit,
		NewLevelBlocksOnGridShow,
		
		SwipeInputInitialize = byte.MaxValue,
	}
}