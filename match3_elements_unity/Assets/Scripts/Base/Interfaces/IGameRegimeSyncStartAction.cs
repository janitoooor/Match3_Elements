namespace Base
{
	public interface IGameRegimeSyncStartAction
	{
		byte priority { get; }
		void Perform();
	}
}