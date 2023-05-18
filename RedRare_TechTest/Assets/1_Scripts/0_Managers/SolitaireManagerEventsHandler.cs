using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SolitaireManagerEventsHandler
{
    public static Action OnStartGame;
    public static void StartGame(this SolitaireManager solitaireManager)
        => OnStartGame?.Invoke();
}
