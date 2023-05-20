using System;

public static class DeckEventsHandler
{
    public static event Action<Deck> OnDeckCreated;
    public static void DeckCreated(this Deck deck) => OnDeckCreated?.Invoke(deck);
}
