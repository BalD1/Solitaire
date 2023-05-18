using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRow : EventHandlerMono
{
    [SerializeField] private int rowID;

    [SerializeField] private float cardsOffset = .25f;

    private Deck deck;

    private Stack<Card> cards;

    private void Reset()
    {
        int.TryParse(this.gameObject.name.Replace("Row_", ""), out rowID);
        cardsOffset = .25f;
    }

    protected override void EventRegister()
    {
        DeckEventsHandler.OnDeckCreated += InitializeRows;
    }

    protected override void EventUnRegister()
    {
        DeckEventsHandler.OnDeckCreated -= InitializeRows;
    }

    private void InitializeRows(Deck _deck)
    {
        deck = _deck;

        List<Card> cardsList = deck.DrawCardMultiple(rowID);

        for (int i = 0; i < cardsList.Count; i++)
        {
            Vector2 pos = this.transform.position;
            pos.y -= cardsOffset * i;

            cardsList[i].transform.position = pos;
            cardsList[i].gameObject.SetActive(true);
        }

        cards = new Stack<Card>(cardsList);
        cards.Peek().SetCardState(recto: true);
    }
}
