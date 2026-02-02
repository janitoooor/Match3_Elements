namespace Common.Saves
{
	public interface ISavesWriter
	{
		void WriteSave<T>(string key, T value);
		T ReadSave<T>(string key, T defaultValue = default);
		void DeleteSave(string key);
		bool HasSave(string key);
		void DeleteAllSaves();
	}
}