using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Pile : EventHandlerMono
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite cardBackSprite;

    private Deck deck;

    private const int BASE_CARDS_IN_PILE = 28;

    private Queue<Card> cardsInPile = new Queue<Card>();


    protected override void EventRegister()
    {
        DeckEventsHandler.OnDeckCreated += CreatePile;
    }

    protected override void EventUnRegister()
    {
        DeckEventsHandler.OnDeckCreated -= CreatePile;
    }

    private void Reset()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    private void CreatePile(Deck _deck)
    {
        deck = _deck;

        // draw cards in deck to create the pile
        cardsInPile = new Queue<Card>(deck.DrawCardMultiple(BASE_CARDS_IN_PILE));

        foreach (var item in cardsInPile)
        {
            item.transform.position = this.transform.position;
        }

        if (cardsInPile.Count > 0)
        {
            spriteRenderer.sprite = cardBackSprite;
            spriteRenderer.enabled = true;
        }
    }

    public Card Draw()
    {
        if (cardsInPile == null || cardsInPile.Count == 0) return null;

        if (cardsInPile.Count <= 1) spriteRenderer.enabled = false;

        return cardsInPile.Dequeue();
    }
}
