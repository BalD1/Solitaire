using UnityEngine;

public abstract class EventHandlerMono : MonoBehaviour
{
    protected virtual void Start()
    {
        EventRegister();
    }

    protected virtual void OnDestroy()
    {
        EventUnRegister();
    }

    protected abstract void EventRegister();
    protected abstract void EventUnRegister();
}
