namespace Base
{
	/// <summary>
	/// Provides a common interface for any game entity which may be
	/// used to start the core game logic when the minimum set of data
	/// to run the game is ready.
	/// </summary>
	public interface IGame
	{
		/// <summary>
		/// Used to start the core game logic.
		/// </summary>
		void Start();
	}
}