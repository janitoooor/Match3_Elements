using UnityEngine;

namespace Core.Input
{
	public delegate void SwipeDelegate(
		SwipeDirectionData swipeDirectionData,
		Vector2 startTouchScreenPosition, 
		Vector2 currentTouchScreenPosition);
	
	public interface ISwipeInputEvent
	{
		event SwipeDelegate OnInputSwipe;
	}
}