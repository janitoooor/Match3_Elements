namespace Core.Level.Configs
{
	public interface ILevelsContainer
	{
		ILevelData GetLevelData(int levelIndex);
		int GetLevelsCount();
	}
}