using Zenject;

namespace Base
{
	/// <summary>
	/// Provides methods to start the core game logic and let the user access all possible game regimes.
	/// </summary>
	public sealed class Game : IGame
	{
		private readonly IGameRegimeLoader gameRegimeLoader;

		[Inject]
		public Game(IGameRegimeLoader gameRegimeLoader)
			=> this.gameRegimeLoader = gameRegimeLoader;

		public void Start()
			=> gameRegimeLoader.LoadDefaultRegime();
	}
}