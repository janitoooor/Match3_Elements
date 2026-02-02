namespace Base
{
	/// <summary>
	/// Provides a common interface for any game entity which may be used
	/// to generate random values.
	/// </summary>
	public interface IRandom
	{
		/// <summary>
		/// Used to get the random int value between min [inclusive] and max [exclusive]
		/// </summary>
		int Between(int min, int max);
        
		/// <summary>
		/// Used to get the random float value between min [inclusive] and max [inclusive]
		/// </summary>
		float Between(float min, float max);
        
		/// <summary>
		/// Used to get the random boolean value.
		/// </summary>
		bool boolean { get; }

		/// <summary>
		/// Used to get the random boolean value by percentage chance.
		/// </summary>
		bool GetChance(int percentage);
	}
}