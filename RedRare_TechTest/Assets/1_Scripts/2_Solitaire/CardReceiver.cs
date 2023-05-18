using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class CardReceiver : EventHandlerMono
{
    [SerializeField] private float cardsOffset = .25f;

    private Stack<Card> cards = new Stack<Card>();

    protected override void EventRegister()
    {
    }

    protected override void EventUnRegister()
    {
    }

    public void ForceLayCard(Card card)
    {
        PlaceCard(card);
    }

    public void TryLayCard(Card card)
    {
        PlaceCard(card);
    }

    private void PlaceCard(Card card)
    {
        cards.Push(card);

        Vector2 pos = this.transform.position;
        pos.y -= cardsOffset * (cards.Count - 1);

        card.StartMovingTo(pos);
    }

    public Card GetNextCard()
    {
        if (cards == null || cards.Count == 0) return null;

        Card cardToSend = cards.Pop();

        if (cards.Count > 0)
            cards.Peek().SetCardState(recto: true);

        return cardToSend;
    }

    public Card PeekNextCard()
    {
        if (cards == null || cards.Count == 0) return null;

        return cards.Peek();
    }
}
