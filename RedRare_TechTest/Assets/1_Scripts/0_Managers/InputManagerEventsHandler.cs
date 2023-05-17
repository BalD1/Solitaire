using System;

public static class InputManagerEventsHandler
{
    public static event Action OnPause;
    public static void Pause(this InputManager inputManager) => OnPause?.Invoke();
}
