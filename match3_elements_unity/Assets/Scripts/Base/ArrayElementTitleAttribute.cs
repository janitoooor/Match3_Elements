using UnityEngine;

namespace Base
{
	/// <summary>
	/// Used to show a readable title inside arrays properties instead
	/// of ElementX.
	/// </summary>
	public sealed class ArrayElementTitleAttribute : PropertyAttribute
	{
		public string varName { get; }
        
		public ArrayElementTitleAttribute(string elementTitleVar)
			=> varName = elementTitleVar;
	}
}