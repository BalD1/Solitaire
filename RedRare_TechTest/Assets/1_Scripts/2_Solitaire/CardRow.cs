using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardReceiver))]
public class CardRow : EventHandlerMono
{
    [SerializeField] private int rowID;

    [SerializeField] private CardReceiver cardReceiver;

    private Deck deck;

    private void Reset()
    {
        int.TryParse(this.gameObject.name.Replace("Row_", ""), out rowID);

        cardReceiver = this.GetComponent<CardReceiver>();
    }

    protected override void Start()
    {
        base.Start();

        // the card candidate must be of the opposite color, and lesser of one
        CardLayConditions_Color layConditions_Color = new CardLayConditions_Color(false, false);
        CardLayConditions_Value cardLayConditions_Value = new CardLayConditions_Value(true);

        cardReceiver.AddLayCondition(layConditions_Color);
        cardReceiver.AddLayCondition(cardLayConditions_Value);
    }

    protected override void EventRegister()
    {
        DeckEventsHandler.OnDeckCreated += GetDeck;
        SolitaireManagerEventsHandler.OnStartGame += InitializeRows;
    }

    protected override void EventUnRegister()
    {
        DeckEventsHandler.OnDeckCreated -= GetDeck;
        SolitaireManagerEventsHandler.OnStartGame -= InitializeRows;
    }

    private void GetDeck(Deck _deck)
    {
        this.deck = _deck;
    }

    private void InitializeRows()
    {
        List<Card> cardsList = deck.DrawCardMultiple(rowID);

        for (int i = 0; i < cardsList.Count; i++)
        {
            cardsList[i].gameObject.SetActive(true);
            cardReceiver.ForceLayCard(cardsList[i]);
        }

        cardReceiver.PeekNextCard()?.SetCardState(recto: true);
    }
}
