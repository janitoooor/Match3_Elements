namespace Core.BlocksMovements
{
	public interface IBlockMovementsPool
	{
		IBlockMovement GetBlockMovement();
		void AddBlockMovement(IBlockMovement blockMovement);
	}
}