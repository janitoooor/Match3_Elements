namespace Base
{
	public interface IGameRegimeSyncStartAction
	{
		int priority { get; }
		void Perform();
	}
}