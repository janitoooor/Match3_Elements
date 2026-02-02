namespace Core.Grid
{
	public delegate void BlocksOnGridFiledClearedDelegate();
	
	public interface IBlocksOnGridFieldClearedEvent
	{
		event BlocksOnGridFiledClearedDelegate OnBlocksOnGridFiledCleared;
	}
}