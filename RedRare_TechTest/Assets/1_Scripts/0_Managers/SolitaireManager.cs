using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolitaireManager : EventHandlerMono
{
    protected override void EventRegister()
    {
    }

    protected override void EventUnRegister()
    {
    }

    public void CallStartGame() => this.StartGame();
}
