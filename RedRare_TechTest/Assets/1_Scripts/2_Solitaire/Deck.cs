using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [field: SerializeField] public Card[] CardsDeck { get; private set; }
    [SerializeField] private Card cardPrefab;

    [SerializeField] private Texture2D cardsSpriteSheet;

    [SerializeField] private Transform cardsParent;


    private const int DECK_SIZE = 52;
    private const int TEXTURE_CARDS_PER_ROW = 13;

    private void Start()
    {
        BuildDeck();
    }

    private void BuildDeck()
    {
        // Get the sprites from the given texture
        Sprite[] spritesData = Resources.LoadAll<Sprite>(cardsSpriteSheet.name);

        CardsDeck = new Card[DECK_SIZE];

        for (int i = 0; i < DECK_SIZE; i++)
        {
            // will return the row of the current card, which we translate by its family
            int cardFamilyIntValue = Mathf.FloorToInt(i / TEXTURE_CARDS_PER_ROW);
            Card.E_CardFamily cardFamily = (Card.E_CardFamily)cardFamilyIntValue;

            Card.CardData cardData = new Card.CardData
            ( 
                _cardFamily: cardFamily,
                _value: i % TEXTURE_CARDS_PER_ROW
            );

            Card card = Instantiate(cardPrefab, cardsParent);
            card.Setup(cardData, spritesData[i]);

#if UNITY_EDITOR
            card.gameObject.name = string.Format($"{card.Data.CardFamily.ToString()} - {card.Data.Value}") ;
#endif

            CardsDeck[i] = card;
        }
    }
}
