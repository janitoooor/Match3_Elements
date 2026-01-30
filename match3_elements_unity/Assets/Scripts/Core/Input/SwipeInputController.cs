using System;
using Base;
using Core.Enums;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Zenject;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Core.Input
{
    public sealed class SwipeInputController : GameRegimeSyncStartAction, 
        ISwipeInputEvent, 
        IDisposable, 
        ITickable
    {
        public event SwipeDelegate OnInputSwipe;
        
        private const float MAX_SWIPE_TIME = 0.5f;
        private const float MIN_SWIPE_TIME = 0.05f;
        
        private readonly InputSystem_Actions actions = new();
        
        private Vector2 startTouchPosition;
        private Vector2 currentTouchPosition;
        
        private bool isTouching;
        private float touchStartTime;
        private float minSwipeDistance;

        private float swipeDuration => Time.time - touchStartTime;
        
        public override int priority => (int)CoreGameRegimeSyncStartActionPriority.SwipeInputInitialize;

        public override void Perform()
        {
            minSwipeDistance = AdaptiveSwipeDetector.CalculateOptimalSwipeDistance();
            actions.UI.Enable();
            EnhancedTouchSupport.Enable();
        }

        public void Tick()
            => HandleSwipe();

        private void HandleSwipe()
        {
            if (Touch.activeTouches.Count > 0)
                HandleActiveTouches();
        }

        private void HandleActiveTouches()
        {
            var touch = Touch.activeTouches[0];

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnTouchBegan(touch);
                    break;
                case TouchPhase.Moved:
                    OnTouchMoved(touch);
                    break;
                case TouchPhase.Ended:
                    OnTouchEnded(touch);
                    break;
                case TouchPhase.Canceled:
                    ResetTouchState();
                    break;
                case TouchPhase.Stationary:
                    OnTouchStationary();
                    break;
            }
        }

        private void OnTouchBegan(Touch touch)
        {
            isTouching = true;
            startTouchPosition = touch.screenPosition;
            currentTouchPosition = startTouchPosition;
            touchStartTime = Time.time;
        }

        private void OnTouchMoved(Touch touch)
        {
            if (!isTouching) 
                return;
            
            currentTouchPosition = touch.screenPosition;
        }

        private void OnTouchEnded(Touch touch)
        {
            if (!isTouching) 
                return;
            
            if (swipeDuration is >= MIN_SWIPE_TIME and <= MAX_SWIPE_TIME)
                TryDetectSwipe(touch);
            else
                ResetTouchState();
        }
        
        private void OnTouchStationary()
        {
            if (swipeDuration > MAX_SWIPE_TIME)
                ResetTouchState();
        }

        private void TryDetectSwipe(Touch touch)
        {
            var endPosition = touch.screenPosition;
            var swipeDelta = endPosition - startTouchPosition;

            if (swipeDelta.magnitude >= minSwipeDistance)
                DetectSwipeDirection(swipeDelta);

            ResetTouchState();
        }
        
        private void ResetTouchState()
        {
            isTouching = false;
            startTouchPosition = Vector2.zero;
            currentTouchPosition = Vector2.zero;
        }

        private void DetectSwipeDirection(Vector2 swipeDelta)
        {
            swipeDelta.Normalize();
            
            var isHorizontalSwipe = Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y);

            var swipeDirectionType = GetSwipeDirectionType(swipeDelta, isHorizontalSwipe);
            
#if UNITY_EDITOR
            Debug.Log($"====> Swipe {swipeDirectionType.ToString()}");
#endif
            OnInputSwipe?.Invoke(GetSwipeDirectionData(swipeDirectionType), startTouchPosition, currentTouchPosition);
        }

        private static SwipeDirectionData GetSwipeDirectionData(SwipeDirectionType swipeDirectionType)
            => new(swipeDirectionType, GetSwipeDirectionVector(swipeDirectionType));

        private static SwipeDirectionType GetSwipeDirectionType(Vector2 swipeDelta, bool isHorizontalSwipe)
            => isHorizontalSwipe 
                ? swipeDelta.x > 0 ? SwipeDirectionType.Right : SwipeDirectionType.Left
                : swipeDelta.y > 0 ? SwipeDirectionType.Up : SwipeDirectionType.Down;

        private static Vector2Int GetSwipeDirectionVector(SwipeDirectionType swipeDirectionType)
            => swipeDirectionType switch
            {
                SwipeDirectionType.Up => Vector2Int.up,
                SwipeDirectionType.Down => Vector2Int.down,
                SwipeDirectionType.Left => Vector2Int.left,
                SwipeDirectionType.Right => Vector2Int.right,
                _ => throw new ArgumentOutOfRangeException(nameof(swipeDirectionType), swipeDirectionType, null)
            };
        
        public void Dispose()
        {
            EnhancedTouchSupport.Disable();
            actions.UI.Disable();
            actions?.Dispose();
        }
    }
}