using System.Collections;
using UnityEngine;

namespace Base
{
	/// <summary>
	/// Provides a common interface for any game entity which may be used to let
	/// the ordinary classes to start coroutines.
	/// </summary>
	public interface IAsyncProcessor
	{
		/// <summary>
		/// Used to starts the specified coroutine.
		/// </summary>
		Coroutine StartCoroutine(IEnumerator routine);
        
		/// <summary>
		/// Used to stop the specified coroutine.
		/// </summary>
		void StopCoroutine(Coroutine routine);
	}
}