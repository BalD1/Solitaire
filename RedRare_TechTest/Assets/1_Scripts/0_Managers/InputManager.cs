using UnityEngine.InputSystem;

public class InputManager : EventHandlerMono
{
    protected override void EventRegister()
    {
    }

    protected override void EventUnRegister()
    {
    }

    private void OnPause(InputValue value)
    {
        if (value.isPressed) this.Pause();
    }
}
