using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardReceiver : MonoBehaviour
{
    [SerializeField] private float cardsOffset = .25f;

    private Stack<Card> cards = new Stack<Card>();

    public void ReceiveCard(Card card)
    {
        cards.Push(card);

        Vector2 pos = this.transform.position;
        pos.y -= cardsOffset * (cards.Count - 1);

        card.StartMovingTo(pos);
    }

    public Card GetNextCard()
    {
        if (cards == null || cards.Count == 0) return null;

        return cards.Pop();
    }

    public Card PeekNextCard()
    {
        if (cards == null || cards.Count == 0) return null;

        return cards.Peek();
    }
}
