using Zenject;

namespace Common.Saves.Level
{
	public sealed class CurrentLevelSavesStorage : ICurrentLevelSavesStorage
	{
		private const string CURRENT_LEVEL_PREFS = "Level";
		
		private readonly ISavesWriter savesWriter;

		[Inject]
		public CurrentLevelSavesStorage(ISavesWriter savesWriter)
			=> this.savesWriter = savesWriter;

		public int currentLevel
		{
			get => savesWriter.ReadSave(CURRENT_LEVEL_PREFS, 0);
			set => savesWriter.WriteSave(CURRENT_LEVEL_PREFS, value);
		}
	}
}