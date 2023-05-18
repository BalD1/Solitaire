using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(SpriteRenderer))]
public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Sprite rectoSprite;
    private Sprite versoSprite;

    // /!\ L'ordre dans l'enum doit respecter l'ordre de la texture Spritesheet
    public enum E_CardFamily
    {
        Spades = 0, // pique
        Clubs = 1, // trèfle
        Diamonds = 2, // carreau
        Hearts = 3,
    }

    [System.Serializable]
    public struct CardData
    {
        public E_CardFamily CardFamily;
        public int Value;

        public CardData(E_CardFamily _cardFamily, int _value)
        {
            CardFamily = _cardFamily;
            Value = _value;
        }
    }

    private CardData data;
    public CardData Data => data;

    public Card(CardData data, Sprite rectoSprite, Sprite versoSprite) => Setup(data, rectoSprite, versoSprite);

    private void Reset()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void Setup(CardData _cardData, Sprite _rectoSprite, Sprite _versoSprite)
    {
        data = _cardData;

        rectoSprite = _rectoSprite;
        versoSprite = _versoSprite;

        SetCardState(recto: false);
    }

    public void SetCardState(bool recto)
    {
        this.spriteRenderer.sprite = recto ? rectoSprite : versoSprite;
    }
}
