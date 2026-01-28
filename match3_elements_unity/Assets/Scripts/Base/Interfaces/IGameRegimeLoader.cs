namespace Base
{
	public interface IGameRegimeLoader
	{
		void LoadDefaultRegime();
		void LoadRegime(GameRegime gameRegime);
	}
}