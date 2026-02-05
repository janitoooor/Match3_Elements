namespace Core.BlocksKill
{
	public delegate void AllBlocksOnGridKilledDelegate();
	
	public interface IAllBlocksOnGridKilledEvent
	{
		event AllBlocksOnGridKilledDelegate OnAllBlocksOnGridKilled;
	}
}