using System.Collections;
using Base;
using Core.Enums;
using Zenject;

namespace Core.Balloons
{
	public sealed class BalloonsAsyncDataInitializer : AsyncDataInitializer
	{
		private const byte MAX_INSTANTIATED_BLOCS_PER_FRAME = 10;
		
		private readonly IBalloonGenerator balloonGenerator;
		private readonly IBalloonsContainer balloonsContainer;
		private readonly IBalloonsRepository balloonsRepository;

		public override byte priority => (byte)CoreAsyncDataInitializePriority.BallonsInitialization;
		
		[Inject]
		public BalloonsAsyncDataInitializer(
			IBalloonGenerator balloonGenerator, 
			IBalloonsContainer balloonsContainer,
			IBalloonsRepository balloonsRepository)
		{
			this.balloonGenerator = balloonGenerator;
			this.balloonsContainer = balloonsContainer;
			this.balloonsRepository = balloonsRepository;
		}
		
		public override IEnumerator Initialize()
		{
			var instantiatedPerFrame = 0;
			
			for (var i = 0; i < balloonsContainer.maxBalloonsCount; i++)
				if (GenerateBalloonAndSkipFrameCheck(ref instantiatedPerFrame)) 
					yield return null;
		}

		private bool GenerateBalloonAndSkipFrameCheck(ref int instantiatedPerFrame)
		{
			balloonsRepository.AddBalloon(balloonGenerator.GenerateBalloon(out var instantiated));

			if (instantiated)
				instantiatedPerFrame++;

			if (instantiatedPerFrame > MAX_INSTANTIATED_BLOCS_PER_FRAME)
			{
				instantiatedPerFrame = 0;
				return true;
			}

			return false;
		}
	}
}