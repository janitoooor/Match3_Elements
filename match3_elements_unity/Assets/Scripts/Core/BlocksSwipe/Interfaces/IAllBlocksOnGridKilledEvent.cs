namespace Core.BlocksSwipe
{
	public delegate void AllBlocksOnGridKilledDelegate();
	
	public interface IAllBlocksOnGridKilledEvent
	{
		event AllBlocksOnGridKilledDelegate OnAllBlocksOnGridKilled;
	}
}