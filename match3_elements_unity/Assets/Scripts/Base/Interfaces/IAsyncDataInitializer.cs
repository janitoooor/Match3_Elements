using System.Collections;

namespace Base
{
	public interface IAsyncDataInitializer
	{
		int priority { get; }
		IEnumerator Initialize();
	}
}