using System.Collections;

namespace Base
{
	public interface IAsyncDataInitializer
	{
		byte priority { get; }
		IEnumerator Initialize();
	}
}