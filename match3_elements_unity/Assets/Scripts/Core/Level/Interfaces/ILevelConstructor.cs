using System.Collections;
using Core.Level.Configs;

namespace Core.Level
{
	public interface ILevelConstructor
	{
		IEnumerator ConstructLevel(ILevelData levelData);
	}
}