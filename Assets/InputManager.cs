using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class InputManager : MonoBehaviour
{
    public JudgementManager judgementManager;
    
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += OnFingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += OnFingerUp;
        TouchSimulation.Enable();
    }
    
    private void OnDisable()
    {
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= OnFingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= OnFingerUp;
        TouchSimulation.Disable();
        EnhancedTouchSupport.Disable();
    }

    private void OnFingerDown(Finger obj)
    {
        var pos = Camera.main!.ScreenToWorldPoint(obj.currentTouch.screenPosition);
        var raycast = Physics2D.Raycast(pos, transform.forward, -1);
        if (!raycast) return;
        if (raycast.transform.TryGetComponent<InputPoint>(out var point))
        {
            judgementManager.OnInput(point.number);
            // print(point.number + "입력됨");
        }
    }
    
    private void OnFingerUp(Finger obj)
    {
        // print(obj.currentTouch.screenPosition);
    }
}