using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Pile : EventHandlerMono, IClickable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite cardBackSprite;
    [SerializeField] private Sprite emptyPileSprite;

    [SerializeField] private CardReceiver cardReceiver;

    public Action onDraw;
    public Action onReset;

    private Deck deck;

    private const int BASE_CARDS_IN_PILE = 28;

    private Stack<Card> cardsInPile = new Stack<Card>();

    [SerializeField] private float pileRefillCooldown = .5f;
    private bool canTakeCard = true;

    protected override void EventRegister()
    {
        DeckEventsHandler.OnDeckCreated += GetDeck;
        SolitaireManagerEventsHandler.OnStartGame += InitializePile;
    }

    protected override void EventUnRegister()
    {
        DeckEventsHandler.OnDeckCreated -= GetDeck;
        SolitaireManagerEventsHandler.OnStartGame -= InitializePile;
    }

    private void Reset()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();

        CardLayConditions_Pile cardLayConditions_Pile = new CardLayConditions_Pile();
        cardReceiver.AddLayCondition(cardLayConditions_Pile);
    }

    private void GetDeck(Deck _deck) => this.deck = _deck;

    private void InitializePile()
    {
        // draw cards in deck to create the pile
        cardsInPile = new Stack<Card>(deck.DrawCardMultiple(BASE_CARDS_IN_PILE));

        foreach (var item in cardsInPile)
        {
            item.transform.position = this.transform.position;
        }

        // the pile sprite
        if (cardsInPile.Count > 0)
        {
            spriteRenderer.sprite = cardBackSprite;
            spriteRenderer.enabled = true;
        }
    }

    /// <summary>
    /// Draw a card from the Pile
    /// </summary>
    /// <returns></returns>
    public Card Draw()
    {
        if (cardsInPile == null || cardsInPile.Count == 0) return null;

        if (cardsInPile.Count <= 1) spriteRenderer.sprite = emptyPileSprite;

        onDraw?.Invoke();

        return cardsInPile.Pop();
    }

    public GameObject GetGameObject() => this.gameObject;

    public void OnMouseInputDown()
    {
        if (!canTakeCard) return;
        
        // if the pile is not empty when clicked, draw a card
        if (cardsInPile.Count > 0)
        {
            Card newCard = Draw();
            newCard.SetCardState(true);
            cardReceiver.ForceLayCard(newCard);
            return;
        }

        canTakeCard = false;

        // else, recreate the pile from the returned cards
        List<Card> cards = new List<Card>(cardReceiver.GetCards());
        cardReceiver.EmptyStack();

        foreach (var item in cards)
        {
            cardsInPile.Push(item);
            item.StopMovements();
            item.transform.position = this.transform.position;
            item.SetCardState(false);
        }

        onReset?.Invoke();

        // little failsafe to prevent errors if the player clicks really fast
        LeanTween.delayedCall(pileRefillCooldown, () => canTakeCard = true);
    }

    public void OnMouseInputUp()
    {
    }
}
