using System;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Card : MonoBehaviour, IClickable
{
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }

    [SerializeField] private float speed = 2f;

    [field: SerializeField] public BoxCollider2D Trigger { get; private set; }

    private CardReceiver cardReceiver;

    private Sprite rectoSprite;
    private Sprite versoSprite;

    private Vector2 targetPos;
    private bool move = false;

    public bool IsOnRecto { get; private set; }

    private Action onMovementEndedAction;

    public Action onCardStateChange;

    // /!\ L'ordre dans l'enum doit respecter l'ordre de la texture Spritesheet
    public enum E_CardFamily
    {
        Spades = 0, // pique
        Clubs = 1, // tr�fle
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

    public const int KING_VALUE = 12;
    public const int QUEEN_VALUE = 11;
    public const int JACK_VALUE = 10;

    private CardData data;
    public CardData Data => data;

    public Card(CardData data, Sprite rectoSprite, Sprite versoSprite) => Setup(data, rectoSprite, versoSprite);

    private void Reset()
    {
        SpriteRenderer = this.GetComponent<SpriteRenderer>();
        Trigger = this.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (move) PerfomMovements();
    }

    public void SetLayerOrder(bool isFirstInList) =>
        SpriteRenderer.sortingOrder = isFirstInList ? 1 : 0; 

    public void Setup(CardData _cardData, Sprite _rectoSprite, Sprite _versoSprite)
    {
        data = _cardData;

        rectoSprite = _rectoSprite;
        versoSprite = _versoSprite;

        SetCardState(recto: false);
    }

    public void SetCardState(bool recto)
    {
        this.SpriteRenderer.sprite = recto ? rectoSprite : versoSprite;
        this.Trigger.enabled = recto ? true : false;
        IsOnRecto = recto;

        onCardStateChange?.Invoke();
    }

    public void StartMovingTo(Vector2 _targetPosition, Action _onMovementEndedAction = null)
    {
        targetPos = _targetPosition;
        onMovementEndedAction = _onMovementEndedAction;

        move = true;
    }

    public void StopMovements() => move = false;

    private void PerfomMovements()
    {
        this.transform.position = Vector2.Lerp(this.transform.position, targetPos, Time.deltaTime * speed);

        // if we arrived at the target position, stop moving
        if (Approximate(this.transform.position, targetPos, .01f))
        {
            onMovementEndedAction?.Invoke();
            onMovementEndedAction = null;

            move = false;
            this.transform.position = targetPos;
        }
    }

    private bool Approximate(Vector2 v1, Vector2 v2, float tolerance)
    {
        return Mathf.Abs(v1.x - v2.x) < tolerance && 
               Mathf.Abs(v1.y - v2.y) < tolerance;
    }

    public static bool CompareColor(E_CardFamily f1,  E_CardFamily f2) 
    {
        if (f1 == f2) return true;

        // TODO : trouver un meilleur moyen ?
        if (f1 == E_CardFamily.Hearts && f2 == E_CardFamily.Diamonds) return true;
        if (f1 == E_CardFamily.Diamonds && f2 == E_CardFamily.Hearts) return true;
        if (f1 == E_CardFamily.Spades && f2 == E_CardFamily.Clubs) return true;
        if (f1 == E_CardFamily.Clubs && f2 == E_CardFamily.Spades) return true;

        return false;
    }

    public void SetCardReceiver(CardReceiver receiver) => cardReceiver = receiver;
    public CardReceiver GetCardReceiver() => cardReceiver;

    public GameObject GetGameObject() => this.gameObject;

    public void OnMouseInputDown()
    {
    }

    public void OnMouseInputUp()
    {
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(Data.CardFamily);
        sb.Append(" - ");
        int displayedVal = Data.Value + 1;
        if (displayedVal <= 10) sb.Append(displayedVal);
        else
        {
            switch (displayedVal)
            {
                case JACK_VALUE:
                    sb.Append("Jack");
                    break;
                case QUEEN_VALUE:
                    sb.Append("Queen");
                    break;
                case KING_VALUE:
                    sb.Append("King");
                    break;
            }
        }

        return sb.ToString();
    }
}
