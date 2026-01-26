namespace Base
{
	/// <summary>
	/// Provides a common interface for any game entity which allows the user to leave the game.
	/// </summary>
	public interface IGameExit
	{
		/// <summary>
		/// Used to leave the game.
		/// </summary>
		void LeaveGame();
	}
}