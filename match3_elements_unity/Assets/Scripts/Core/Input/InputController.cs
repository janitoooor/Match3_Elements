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
    public sealed class InputController : GameRegimeSyncStartAction, 
        IInputController, 
        IDisposable, 
        ITickable
    {
        public event SwipeDelegate OnSwipe;
        
        private const float MAX_SWIPE_TIME = 0.5f;
        private const float MIN_SWIPE_TIME = 0.05f;
        
        private readonly InputSystem_Actions actions = new();
        
        private Vector2 startTouchPosition;
        private Vector2 currentTouchPosition;
        
        private bool isTouching;
        private float touchStartTime;
        private float minSwipeDistance;

        private float swipeDuration => Time.time - touchStartTime;
        
        public override int priority => (int)CoreGameRegimeSyncStartActionPriority.Input;

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
        
            if (isHorizontalSwipe)
                CallSwipeEvent(swipeDelta.x > 0 ? SwipeDirection.Right : SwipeDirection.Left);
            else
                CallSwipeEvent(swipeDelta.y > 0 ? SwipeDirection.Up : SwipeDirection.Down);
        }
        
        private void CallSwipeEvent(SwipeDirection swipeDirection)
        {
#if UNITY_EDITOR
            Debug.Log($"====> Swipe {swipeDirection.ToString()}");
#endif
            OnSwipe?.Invoke(swipeDirection, startTouchPosition, currentTouchPosition);
        }

        public void Dispose()
        {
            EnhancedTouchSupport.Disable();
            actions.UI.Disable();
            actions?.Dispose();
        }
    }
}