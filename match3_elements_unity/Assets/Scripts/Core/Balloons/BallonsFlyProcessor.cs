using System.Collections.Generic;
using Base;
using UnityEngine;
using Zenject;

namespace Core.Balloons
{
	public sealed class BallonsFlyProcessor : IBallonsFlyProcessor, IBalloonsRepository,  ITickable
	{
		private readonly Camera camera;
		private readonly IRandom random;
		private readonly IBalloonsContainer balloonsContainer;
		
		private readonly List<IBalloonEntity> balloons = new ();

		private float spawnBalloonsCd;

		private bool isStarted;
		
		[Inject]
		public BallonsFlyProcessor(Camera camera, IRandom random, IBalloonsContainer balloonsContainer)
		{
			this.camera = camera;
			this.random = random;
			this.balloonsContainer = balloonsContainer;
		}

		public void Start()
			=> isStarted = true;

		public void Tick()
		{
			if (isStarted)
				TryMoveBalloons();
		}

		private void TryMoveBalloons()
		{
			foreach (var balloon in balloons)
				TryMoveBalloon(balloon);
			
			spawnBalloonsCd -= Time.deltaTime;
		}

		private void TryMoveBalloon(IBalloonEntity balloon)
		{
			if (!balloon.isLaunched)
				LaunchBalloon(balloon);
			else
				MoveBalloon(balloon);
		}

		private void MoveBalloon(IBalloonEntity balloon)
		{
			balloon.timer += Time.deltaTime;
			
			var x = balloon.startPos.x + balloon.speed * balloon.timer;

			x *= balloon.isMoveFromRight ? -1 : 1;
			
			var screenBoundXPos = GetLaunchPosX(!balloon.isMoveFromRight, out _);
			
			if (balloon.isMoveFromRight ? x < screenBoundXPos : x > screenBoundXPos)
				LaunchBalloon(balloon);
			else
				MoveBalloon(balloon, x);
		}

		private void MoveBalloon(IBalloonEntity balloon, float x)
		{
			var waveTime = balloon.timer * balloonsContainer.frequency;
			var waveValue = balloonsContainer.waveCurve.Evaluate(Mathf.PingPong(waveTime, 1f));
			
			var y = balloon.startPos.y + waveValue * balloonsContainer.amplitude;
    
			balloon.SetPos(new Vector2(x, y));
		}

		public void AddBalloon(IBalloonEntity balloon)
			=> balloons.Add(balloon);
		
		private void LaunchBalloon(IBalloonEntity balloon)
		{
			if (spawnBalloonsCd > 0)
				return;
			
			var fromRight = random.Between(0, 2) == 0;
			
			balloon.Launch(
				GetLaunchPosition(fromRight), 
				GetRandomBalloonSprite(), 
				GetRandomBalloonSpeed(), 
				fromRight);

			spawnBalloonsCd = random.Between(balloonsContainer.spawnBalloonsCdMin, balloonsContainer.spawnBalloonsCdMax);
		}

		private float GetRandomBalloonSpeed()
			=> random.Between(balloonsContainer.minSpeed, balloonsContainer.maxSpeed);

		private Sprite GetRandomBalloonSprite()
			=> balloonsContainer.balloonSprites[random.Between(0, balloonsContainer.balloonSprites.Count)];

		private Vector3 GetLaunchPosition(bool fromRight)
		{
			var spawnX = GetLaunchPosX(fromRight, out var screenRightTop);
			var spawnY = GetLaunchPosY(screenRightTop);

			return new Vector3(spawnX, spawnY, 0);
		}

		private float GetLaunchPosY(Vector3 screenRightTop)
			=> random.Between(
				screenRightTop.y / balloonsContainer.spawnDivideOffsetY, 
				screenRightTop.y - balloonsContainer.spawnOffsetYTop);

		private float GetLaunchPosX(bool fromRight, out Vector3 screenRightTop)
		{
			var offsetX = balloonsContainer.spawnOffsetX;
			
			var screenLeftBot = camera.ViewportToWorldPoint(Vector2.zero);
			screenRightTop = camera.ViewportToWorldPoint(Vector2.one);
			
			return fromRight ? screenRightTop.x + offsetX : screenLeftBot.x - offsetX;
		}
	}
}