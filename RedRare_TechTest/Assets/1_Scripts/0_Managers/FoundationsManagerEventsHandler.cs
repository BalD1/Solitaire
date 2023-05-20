using System;

public static class FoundationsManagerEventsHandler
{
    public static event Action<int, bool> OnFoundationCompeltedStateChange;
    public static void FoundationCompletedStateChange(this FoundationsManager foundationsManager, int id, bool newState) 
        => OnFoundationCompeltedStateChange?.Invoke(id, newState);

    public static event Action OnEveryFoundationCompleted;
    public static void EveryFoundationCompleted(this FoundationsManager foundationsManager) => OnEveryFoundationCompleted?.Invoke();
}
