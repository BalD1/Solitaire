using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class CardReceiver : EventHandlerMono, IClickable
{
    [SerializeField] private float cardsOffset = .25f;

    [SerializeField] private List<CardLayConditions_Base> cardLayConditions = new List<CardLayConditions_Base>();

    private Stack<Card> cards = new Stack<Card>();

    public event Action<Card> OnRemoveCard;
    public event Action<Card> OnLayCard;

    protected override void EventRegister()
    {
        CardGrabManagerEventsHandler.OnPlacedCard += OnPlacedCard;
    }

    protected override void EventUnRegister()
    {
        CardGrabManagerEventsHandler.OnPlacedCard -= OnPlacedCard;
    }


    public void AddLayCondition(CardLayConditions_Base layCondition) => cardLayConditions.Add(layCondition);
    public void RemoveLayCondition(CardLayConditions_Base layCondition) => cardLayConditions.Remove(layCondition);

    public void ForceLayCard(Card card)
    {
        PlaceCard(card);
    }

    public bool TryLayCard(Card card)
    {
        foreach (var item in cardLayConditions)
            if (item.CheckCandidateCard(card) == false) return false;

        Card lastCard = PeekNextCard();
        if (lastCard != null)
        {
            foreach (var item in cardLayConditions)
                if (item.CheckLastCard(card, lastCard) == false) return false;
        }

        PlaceCard(card);

        return true;
    }

    private void PlaceCard(Card card)
    {
        cards.Push(card);

        card.gameObject.SetActive(true);

        Vector2 pos = this.transform.position;
        pos.y -= cardsOffset * (cards.Count - 1);

        card.StartMovingTo(pos);

        card.SetCardReceiver(this);

        OnLayCard?.Invoke(card);
    }

    public int GetCardsCount() => cards.Count;
    public Stack<Card> GetCards() => cards;

    public void EmptyStack() => cards.Clear();

    public Card GetNextCard()
    {
        if (cards == null || cards.Count == 0) return null;

        Card cardToSend = cards.Pop();

        OnRemoveCard?.Invoke(cardToSend);

        return cardToSend;
    }

    public Card PeekNextCard()
    {
        if (cards == null || cards.Count == 0) return null;

        return cards.Peek();
    }

    private void OnPlacedCard(Card card, CardReceiver receiver)
    {
        if (receiver == this) return;

        if (cards.Count == 0) return;

        cards.Peek().SetCardState(recto: true);
    }

    public GameObject GetGameObject() => this.gameObject;

    public void OnMouseInputDown() { }

    public void OnMouseInputUp() { }
}
