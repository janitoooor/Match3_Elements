using UnityEngine;

namespace Core.Balloons
{
	public interface IBalloonEntity
	{
		float timer { get; set; }
		float speed { get; }
		Vector2 startPos { get; }
		bool isMoveFromRight { get; }
		bool isLaunched { get; }
		void Launch(Vector2 launchPos, Sprite sprite, float newSpeed, bool moveFromRight);
		void SetPos(Vector2 pos);
	}
}