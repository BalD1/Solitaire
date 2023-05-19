using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardReceiver))]
public class Foundation : EventHandlerMono
{
    [SerializeField] private CardReceiver cardReceiver;

    private CardLayConditions_Value emptyReceiverCondition;

    [SerializeField, ReadOnly] private bool completed;
    public bool IsCompleted => completed;

    public Action<int, bool> OnCompletedStateChange = null;

    public int ID { get; private set; }

    private void Reset()
    {
        cardReceiver = this.GetComponent<CardReceiver>();
    }

    public void Setup(int id) => ID = id;

    protected override void EventRegister()
    {
        cardReceiver.OnLayCard += OnLayedCard;
        cardReceiver.OnRemoveCard += OnRemovedCard;
    }

    protected override void EventUnRegister()
    {
        cardReceiver.OnLayCard -= OnLayedCard;
        cardReceiver.OnRemoveCard -= OnRemovedCard;
    }

    protected override void Start()
    {
        base.Start();

        completed = false;

        CardLayConditions_Color layConditions_Color = new CardLayConditions_Color(true, true);
        CardLayConditions_Value cardLayConditions_Value = new CardLayConditions_Value(false);

        // when the foundation is empty, force the first card to be an ace
        emptyReceiverCondition = new CardLayConditions_Value(false, 0);

        cardReceiver.AddLayCondition(layConditions_Color);
        cardReceiver.AddLayCondition(cardLayConditions_Value);
        cardReceiver.AddLayCondition(emptyReceiverCondition);
    }

    private void OnLayedCard(Card card)
    {
        cardReceiver.RemoveLayCondition(emptyReceiverCondition);

        if (card.Data.Value == Card.KING_VALUE)
        {
            completed = true;
            OnCompletedStateChange?.Invoke(ID, completed);
        }
    }

    private void OnRemovedCard(Card card)
    {
        if (cardReceiver.GetCardsCount() == 0) cardReceiver.AddLayCondition(emptyReceiverCondition);

        if (card.Data.Value == Card.KING_VALUE)
        {
            completed = false;
            OnCompletedStateChange?.Invoke(ID, completed);
        }
    }
}
