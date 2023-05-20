using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolitaireManager : EventHandlerMono
{
#if UNITY_EDITOR
    [InspectorButton(nameof(CallWin), ButtonWidth = 200)]
    [SerializeField] private bool EDITOR_forceWin;
#endif

    private float gameStartTime;

    protected override void EventRegister()
    {
        FoundationsManagerEventsHandler.OnEveryFoundationCompleted += CallWin;
    }

    protected override void EventUnRegister()
    {
        FoundationsManagerEventsHandler.OnEveryFoundationCompleted -= CallWin;
    }

    public void CallStartGame()
    {
        this.StartGame();
        this.gameStartTime = Time.timeSinceLevelLoad;

        if (DataKeeper.HaveValidProfile())
            DataKeeper.CurrentProfile.playedGames++;
    }

    private void CallWin()
    {
        this.Win();

        float gameTime = Time.timeSinceLevelLoad - gameStartTime;

        if (DataKeeper.HaveValidProfile())
            if (DataKeeper.CurrentProfile.fastestGame > gameTime || DataKeeper.CurrentProfile.fastestGame == -1)
                DataKeeper.CurrentProfile.fastestGame = gameTime;
    }
}
