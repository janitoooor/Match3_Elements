using System;

namespace Base
{
	/// <summary>
	/// Provides methods to generate random values.
	/// </summary>
	public sealed class UnityRandom : IRandom
	{
		/// <summary>
		/// Used to get the random int value between min [inclusive] and max [exclusive]
		/// </summary>
		public int Between(int min, int max) => UnityEngine.Random.Range(min, max);

		/// <summary>
		/// Used to get the random float value between min [inclusive] and max [inclusive]
		/// </summary>
		public float Between(float min, float max) => UnityEngine.Random.Range(min, max);

		/// <summary>
		/// Used to get the random boolean value.
		/// </summary>
		public bool boolean => Between(0, 2) == 0;

		/// <summary>
		/// Used to get the random boolean value by percentage chance.
		/// </summary>
		public bool GetChance(int percentage)
		{
			if (percentage is < 0 or > 100)
				throw new ArgumentException("Percentage must be between 0 and 100 inclusive.");

			var randomNumber = Between(1, 101);
			return randomNumber <= percentage;
		}
	}
}