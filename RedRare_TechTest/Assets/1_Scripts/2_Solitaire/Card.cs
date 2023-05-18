using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private float speed = 2f;

    private Sprite rectoSprite;
    private Sprite versoSprite;

    private Vector2 targetPos;
    private bool move = false;

    // /!\ L'ordre dans l'enum doit respecter l'ordre de la texture Spritesheet
    public enum E_CardFamily
    {
        Spades = 0, // pique
        Clubs = 1, // trèfle
        Diamonds = 2, // carreau
        Hearts = 3,
    }

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

    private void Update()
    {
        if (move) PerfomMovements();
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

    public void StartMovingTo(Vector2 targetPosition)
    {
        targetPos = targetPosition;
        move = true;
    }

    private void PerfomMovements()
    {
        this.transform.position = Vector2.Lerp(this.transform.position, targetPos, Time.deltaTime * speed);

        // if we arrived at the target position, stop moving
        if (Approximate(this.transform.position, targetPos, .01f))
        {
            move = false;
            this.transform.position = targetPos;
        }
    }

    private bool Approximate(Vector2 v1, Vector2 v2, float tolerance)
    {
        return Mathf.Abs(v1.x - v2.x) < tolerance && 
               Mathf.Abs(v1.y - v2.y) < tolerance;
    }
}
