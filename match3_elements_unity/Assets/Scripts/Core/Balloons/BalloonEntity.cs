using UnityEngine;

namespace Core.Balloons
{
	public sealed class BalloonEntity : MonoBehaviour, IBalloonEntity
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;
		
		public float speed { get; private set; } = 1f;
		public Vector2 startPos { get; private set; }
		public bool isMoveFromRight { get; private set; }
		public bool isLaunched { get; private set; }
		public float timer { get; set; }
		
		public void Launch(Vector2 launchPos, Sprite sprite, float newSpeed, bool moveFromRight)
		{
			startPos = launchPos;
			SetPos(startPos);
			
			spriteRenderer.sprite = sprite;
			isMoveFromRight = moveFromRight;
			speed = newSpeed;
			timer = 0f;
			isLaunched = true;
		}

		public void SetPos(Vector2 pos)
			=> transform.position = pos;
	}
}