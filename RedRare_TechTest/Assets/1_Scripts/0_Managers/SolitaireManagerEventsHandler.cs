using System;

public static class SolitaireManagerEventsHandler
{
    public static event Action OnStartGame;
    public static void StartGame(this SolitaireManager solitaireManager)
        => OnStartGame?.Invoke();

    public static event Action OnWin;
    public static void Win(this SolitaireManager solitaireManager) => OnWin?.Invoke();
}
