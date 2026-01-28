using System;

namespace Base
{
	public interface IPrefabsContainer<in T> where T : Enum
	{
		TP GetPrefab<TP>(T prefabKey);
	}
}