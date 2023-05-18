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

    protected override void Start()
    {
        base.Start();

        this.StartGame();
    }
}
