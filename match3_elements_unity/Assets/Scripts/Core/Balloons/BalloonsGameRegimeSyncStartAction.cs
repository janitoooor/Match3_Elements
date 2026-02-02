using Base;
using Core.Enums;
using Zenject;

namespace Core.Balloons
{
	public sealed class BalloonsGameRegimeSyncStartAction : GameRegimeSyncStartAction
	{
		private readonly IBallonsFlyProcessor flyProcessor;
		public override byte priority => (byte)CoreGameRegimeSyncStartActionPriority.BalloonActivate;

		[Inject]
		public BalloonsGameRegimeSyncStartAction(IBallonsFlyProcessor flyProcessor)
			=> this.flyProcessor = flyProcessor;

		public override void Perform()
			=> flyProcessor.Start();
	}
}