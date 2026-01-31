using System.Collections;
using Base;
using Core.Enums;
using Zenject;

namespace Core.Level
{
	public sealed class LevelAsyncDataInitializer : AsyncDataInitializer
	{
		private readonly ICurrentLevelDataProvider currentLevelDataProvider;
		private readonly ILevelConstructor levelConstructor;

		public override byte priority => (byte)CoreAsyncDataInitializePriority.LevelInitialization;
		
		[Inject]
		public LevelAsyncDataInitializer(
			ICurrentLevelDataProvider currentLevelDataProvider, 
			ILevelConstructor levelConstructor)
		{
			this.currentLevelDataProvider = currentLevelDataProvider;
			this.levelConstructor = levelConstructor;
		}
		
		public override IEnumerator Initialize()
		{
			yield return levelConstructor.ConstructLevel(currentLevelDataProvider.GetCurrentLevelData());
		}
	}
}