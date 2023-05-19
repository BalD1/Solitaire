using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolitaireManager : EventHandlerMono
{
#if UNITY_EDITOR
    [InspectorButton(nameof(CallWin), ButtonWidth = 200)]
    [SerializeField] private bool EDITOR_forceWin;
#endif

    protected override void EventRegister()
    {
        FoundationsManagerEventsHandler.OnEveryFoundationCompleted += CallWin;
    }

    protected override void EventUnRegister()
    {
        FoundationsManagerEventsHandler.OnEveryFoundationCompleted -= CallWin;
    }

    public void CallStartGame() => this.StartGame();

    private void CallWin() => this.Win();
}
