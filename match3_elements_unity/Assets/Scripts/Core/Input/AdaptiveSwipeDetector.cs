using UnityEngine;

namespace Core.Input
{
	public static class AdaptiveSwipeDetector
	{
		private const float SCREEN_PERCENTAGE = 0.05f;
		private const float MIN_ABSOLUTE_PIXELS = 50f;
		private const float MAX_ABSOLUTE_PIXELS = 200f;
    
		public static float CalculateOptimalSwipeDistance()
		{
			var screenBased = Screen.width * SCREEN_PERCENTAGE;
        
			screenBased = Mathf.Clamp(screenBased, MIN_ABSOLUTE_PIXELS, MAX_ABSOLUTE_PIXELS);
			
			Debug.Log($"====> Set Optimal swipe distance: {screenBased}px " + $"(Screen: {Screen.width}x{Screen.height}, DPI: {Screen.dpi})");
        
			return screenBased;
		}
	}
}