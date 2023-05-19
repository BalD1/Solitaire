using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class CardReceiver : EventHandlerMono, IClickable
{
    [SerializeField] private float cardsOffset = .25f;

    [SerializeField] private BoxCollider2D trigger;

    [SerializeField] private List<CardLayConditions_Base> cardLayConditions = new List<CardLayConditions_Base>();

    private Stack<Card> cards = new Stack<Card>();

    public event Action<Card> OnRemoveCard;
    public event Action<Card> OnLayCard;

    private void Reset()
    {
        trigger = this.GetComponent<BoxCollider2D>();
    }

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

        ChangeCollider(addedCard: true);

        OnLayCard?.Invoke(card);
    }

    public int GetCardsCount() => cards.Count;
    public Stack<Card> GetCards() => cards;

    public void EmptyStack() => cards.Clear();

    public Card GetNextCard()
    {
        if (cards == null || cards.Count == 0) return null;

        Card cardToSend = cards.Pop();

        ChangeCollider(addedCard: false);

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

    /// <summary>
    /// Change the receiver's collider when we add or remove a card.
    /// </summary>
    /// <param name="addedCard"></param>
    private void ChangeCollider(bool addedCard)
    {
        // add, or remove, the cards offset to the size
        Vector2 newTriggerSize = this.trigger.size;
        newTriggerSize.y = addedCard ?
                           newTriggerSize.y + cardsOffset :
                           newTriggerSize.y - cardsOffset;

        // add, or remove, the cards offset to the offset
        float triggerOffset = cardsOffset / 2;
        Vector2 newTriggerOffset = this.trigger.offset;
        newTriggerOffset.y = addedCard ? 
                             newTriggerOffset.y - triggerOffset :
                             newTriggerOffset.y + triggerOffset;

        this.trigger.offset = newTriggerOffset;
        this.trigger.size = newTriggerSize;
    }

    public GameObject GetGameObject() => this.gameObject;

    public void OnMouseInputDown() { }

    public void OnMouseInputUp() { }
}
