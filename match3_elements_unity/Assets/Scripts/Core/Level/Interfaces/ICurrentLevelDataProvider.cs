using Core.Level.Configs;

namespace Core.Level
{
	public interface ICurrentLevelDataProvider
	{
		ILevelData GetCurrentLevelData();
	}
}