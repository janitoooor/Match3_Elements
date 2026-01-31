namespace Core.Level
{
	public interface ICurrentLevelProvider
	{
		int currentLevel { get; }
		void ChangeCurrentLevel();
	}
}