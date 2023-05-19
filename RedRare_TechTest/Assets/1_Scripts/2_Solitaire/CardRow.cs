using System.Collections;
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

        cardReceiver?.PeekNextCard().SetCardState(recto: true);
    }

    public GameObject GetGameObject() => this.gameObject;
}
