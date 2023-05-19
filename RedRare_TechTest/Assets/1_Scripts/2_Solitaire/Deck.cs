using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : EventHandlerMono
{
    [SerializeField] private Card[] cardsDeck;
    [SerializeField] private Card cardPrefab;
    private Queue<Card> cardsQueue;

    [SerializeField] private Texture2D cardsSpriteSheet;

    [SerializeField] private Transform cardsParent;

    [SerializeField] private Sprite cardsVersoSprite;

    private const int DECK_SIZE = 52;
    private const int TEXTURE_CARDS_PER_ROW = 13;

    protected override void EventRegister()
    {
    }

    protected override void EventUnRegister()
    {
    }

    protected override void Start()
    {
        base.Start();

        BuildDeck();
    }

    /// <summary>
    /// Automaticaly builds the deck by creating cards from a prefab, then getting the right sprite from a texture
    /// </summary>
    private void BuildDeck()
    {
        // Get the sprites from the given texture
        Sprite[] spritesData = Resources.LoadAll<Sprite>(cardsSpriteSheet.name);

        cardsDeck = new Card[DECK_SIZE];

        for (int i = 0; i < DECK_SIZE; i++)
        {
            // will return the row of the current card in the texture, which we then translate by its family
            int cardFamilyIntValue = Mathf.FloorToInt(i / TEXTURE_CARDS_PER_ROW);
            Card.E_CardFamily cardFamily = (Card.E_CardFamily)cardFamilyIntValue;

            Card.CardData cardData = new Card.CardData
            ( 
                _cardFamily: cardFamily,
                _value: i % TEXTURE_CARDS_PER_ROW
            );

            // Create and setup the gameobject
            Card card = Instantiate(cardPrefab, cardsParent);

            card.Setup
            (
                _cardData: cardData,
                _rectoSprite: spritesData[i], 
                _versoSprite: cardsVersoSprite
            );

#if UNITY_EDITOR
            // for debug ease only; not useful in build
            card.gameObject.name = card.ToString();
#endif

            card.gameObject.SetActive(false);

            cardsDeck[i] = card;
        }

        cardsDeck.Shuffle(cardsDeck.Length);

        cardsQueue = new Queue<Card>(cardsDeck);

        this.DeckCreated();
    }

    /// <summary>
    /// Draw a card from <seealso cref="cardsQueue"/>.
    /// </summary>
    /// <returns> null if the queue is empty, next <seealso cref="Card"/> otherwise </returns>
    public Card DrawCard()
    {
        if (cardsQueue == null || cardsQueue.Count == 0) return null;

        return cardsQueue.Dequeue();
    }

    /// <summary>
    /// Draw <paramref name="amount"/> cards from <seealso cref="cardsQueue"/>.
    /// </summary>
    /// <returns> null if the queue is empty, next "<paramref name="amount"/>" <seealso cref="Card"/>s otherwise </returns>
    public List<Card> DrawCardMultiple(int amount)
    {
        List<Card> cards = new List<Card>();

        if (amount == 0) return cards;
        if (cardsQueue == null || cardsQueue.Count == 0) return cards;

        while (amount > 0 && cardsQueue.Count > 0)
        {
            cards.Add(cardsQueue.Dequeue());
            amount--;
        }

        return cards;
    }
}
