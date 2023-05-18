using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(SpriteRenderer))]
public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public enum E_CardFamily
    {
        Spades = 0, // pique
        Clubs = 1, // tr�fle
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

    public Card(CardData data, Sprite sprite) => Setup(data, sprite);

    private void Reset()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void Setup(CardData cardData, Sprite sprite)
    {
        data = cardData;

        this.spriteRenderer.sprite = sprite;
    }
}