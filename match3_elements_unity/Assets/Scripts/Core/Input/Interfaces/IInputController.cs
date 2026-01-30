using Core.Enums;
using UnityEngine;

namespace Core.Input
{
	public delegate void SwipeDelegate(
		SwipeDirection swipeDirection,
		Vector2 startTouchScreenPosition, 
		Vector2 currentTouchScreenPosition);
	
	public interface IInputController
	{
		event SwipeDelegate OnSwipe;
	}
}