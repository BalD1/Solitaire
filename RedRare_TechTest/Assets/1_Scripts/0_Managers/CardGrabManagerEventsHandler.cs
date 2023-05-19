
using System;

public static class CardGrabManagerEventsHandler
{
    public static event Action<Card> OnGrabCard;
    public static void GrabCard(this CardGrabManager cardGrabManager, Card card) => OnGrabCard?.Invoke(card);

    public static event Action<Card, CardReceiver> OnPlacedCard;
    public static void PlacedCard(this  CardGrabManager cardGrabManager, Card card, CardReceiver receiver) => OnPlacedCard?.Invoke(card, receiver);
}
