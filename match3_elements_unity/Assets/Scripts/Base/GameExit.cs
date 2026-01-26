namespace Base
{
	/// <summary>
	/// Provides methods to leave the game.
	/// </summary>
	public sealed class GameExit : IGameExit
	{
		public void LeaveGame()
			=> ExitGame();

		private static void ExitGame()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
		}
	}
}