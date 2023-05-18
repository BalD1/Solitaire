using UnityEngine;
using System;

public static class InputManagerEventsHandler
{
    public static event Action OnPause;
    public static void Pause(this InputManager inputManager) => OnPause?.Invoke();

    public static event Action<Vector2> OnMouseDown;
    public static void MouseDown(this InputManager inputManager, Vector2 mousePos) => OnMouseDown?.Invoke(mousePos);

    public static event Action<Vector2> OnMouseUp;
    public static void MouseUp(this InputManager inputManager, Vector2 mousePos) => OnMouseUp?.Invoke(mousePos);

    public static event Action<IClickable> OnClickableDown;
    public static void ClickableDown(this InputManager inputManager, IClickable clickable) => OnClickableDown?.Invoke(clickable);

    public static event Action<IClickable> OnClickableUp;
    public static void ClickableUp(this InputManager inputManager, IClickable clickable) => OnClickableUp?.Invoke(clickable);
}
