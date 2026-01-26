using UnityEngine;
using UnityEngine.SceneManagement;

namespace Base
{
	/// <summary>
	/// Used for init some sdk before game launch
	/// </summary>
	public sealed class Launcher : MonoBehaviour
	{
		private void Awake()
			=> SceneManager.LoadScene("Main");
	}
}
