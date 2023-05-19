using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Pile : EventHandlerMono, IClickable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite cardBackSprite;
    [SerializeField] private Sprite emptyPileSprite;

    [SerializeField] private CardReceiver cardReceiver;

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

        if (cardsInPile.Count > 0)
        {
            spriteRenderer.sprite = cardBackSprite;
            spriteRenderer.enabled = true;
        }
    }

    public Card Draw()
    {
        if (cardsInPile == null || cardsInPile.Count == 0) return null;

        if (cardsInPile.Count <= 1) spriteRenderer.sprite = emptyPileSprite;

        return cardsInPile.Pop();
    }

    public GameObject GetGameObject() => this.gameObject;

    public void OnMouseInputDown()
    {
        if (!canTakeCard) return;

        if (cardsInPile.Count > 0)
        {
            Card newCard = Draw();
            newCard.SetCardState(true);
            cardReceiver.ForceLayCard(newCard);
            return;
        }

        canTakeCard = false;

        List<Card> cards = new List<Card>(cardReceiver.GetCards());
        cardReceiver.EmptyStack();

        foreach (var item in cards)
        {
            cardsInPile.Push(item);
            item.StopMovements();
            item.transform.position = this.transform.position;
            item.SetCardState(false);
        }

        LeanTween.delayedCall(pileRefillCooldown, () => canTakeCard = true);
    }

    public void OnMouseInputUp()
    {
    }
}
