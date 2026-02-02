using UnityEditor;
using UnityEngine;

namespace Base
{
	public sealed class PlayerPrefsSavesCleaner
	{
#if UNITY_EDITOR
		[MenuItem("Tools/Clear Player Prefs Saves")]
		private static void ClearPlayerPrefsSaves()
		{
			PlayerPrefs.DeleteAll();
			PlayerPrefs.Save();
		}
#endif
	}
}