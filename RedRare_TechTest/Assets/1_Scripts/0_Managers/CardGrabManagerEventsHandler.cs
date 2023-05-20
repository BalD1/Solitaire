
using System;
using System.Collections.Generic;

public static class CardGrabManagerEventsHandler
{
    public static event Action<List<Card>> OnGrabCard;
    public static void GrabCard(this CardGrabManager cardGrabManager, List<Card> cards) => OnGrabCard?.Invoke(cards);

    public static event Action<Card, CardReceiver> OnPlacedCard;
    public static void PlacedCard(this  CardGrabManager cardGrabManager, Card card, CardReceiver receiver) => OnPlacedCard?.Invoke(card, receiver);
}
