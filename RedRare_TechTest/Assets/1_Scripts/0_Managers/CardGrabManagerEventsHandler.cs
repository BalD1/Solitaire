
using System;

public static class CardGrabManagerEventsHandler
{
    public static event Action<Card> OnGrabCard;
    public static void GrabCard(this CardGrabManager cardGrabManager, Card card) => OnGrabCard?.Invoke(card);
}
