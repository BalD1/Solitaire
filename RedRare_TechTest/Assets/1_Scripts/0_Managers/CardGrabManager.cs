using System.Collections.Generic;
using UnityEngine;

public class CardGrabManager : EventHandlerMono
{
    [SerializeField] private float cardsGrabOffset = .25f;

    private List<Card> grabbedCards = new List<Card>();

    private Camera cam = null;

    private CardReceiver grabbedCardBaseReceiver = null;

    protected override void EventRegister()
    {
        InputManagerEventsHandler.OnClickableDown += OnClickDown;
        InputManagerEventsHandler.OnClickableUp += OnClickabkleUp;
        InputManagerEventsHandler.OnMouseUp += OnMouseInputUp;
    }

    protected override void EventUnRegister()
    {
        InputManagerEventsHandler.OnClickableDown -= OnClickDown;
        InputManagerEventsHandler.OnClickableUp -= OnClickabkleUp;
        InputManagerEventsHandler.OnMouseUp -= OnMouseInputUp;
    }

    protected override void Start()
    {
        base.Start();

        cam = Camera.main;
    }

    private void Update()
    {
        if (grabbedCards.Count > 0) MoveGrabbedCardToMouse();
    }

    private void MoveGrabbedCardToMouse()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector2 grabPos = mousePos;
        for (int i = 0; i < grabbedCards.Count; i++)
        {
            grabPos.y = mousePos.y + (cardsGrabOffset * i);
            grabbedCards[i].StartMovingTo(grabPos);
        }
    }

    private void OnClickDown(IClickable clickable)
    {
        // ignore if already grabbing cards
        if (grabbedCards.Count > 0) return;

        // Check if the clickable is a card
        Card clickedOnCard = clickable.GetGameObject().GetComponent<Card>();
        if (clickedOnCard == null) return;

        // get the card receiver
        grabbedCardBaseReceiver = clickedOnCard.GetCardReceiver();
        if (grabbedCardBaseReceiver != null)
            // get every cards until the one we clicked on
            grabbedCards = grabbedCardBaseReceiver.GetEveryCardsTo(clickedOnCard);
        else
            grabbedCards = new List<Card>() { clickedOnCard};

        foreach (var item in grabbedCards)
        {
            item.Trigger.enabled = false;
            item.SpriteRenderer.sortingLayerName = SortingLayersNames.FG_LOWEST;
        }

        this.GrabCard(grabbedCards);
    }

    private void OnClickabkleUp(IClickable clickable)
    {
        // ignore if we aren't grabbing cards
        if (grabbedCards.Count == 0) return;

        // check if we clicked on a card or on a receiver
        Card card = clickable.GetGameObject().GetComponent<Card>();
        CardReceiver cardReceiver = card != null ? card.GetCardReceiver() :
                                    clickable.GetGameObject().GetComponent<CardReceiver>();
        if (cardReceiver == null) return;

        // place every grabbed card on the receiver
        for (int i = grabbedCards.Count - 1; i >= 0; i--)
        {
            if (!cardReceiver.TryLayCard(grabbedCards[i])) return;
            this.PlacedCard(grabbedCards[i], cardReceiver);
        }

        ResetGrabbedCard();
    }

    private void OnMouseInputUp(Vector2 mousePos)
    {
        if (grabbedCards.Count == 0) return;

        // if the user grabbed card but didn't released input on a receiver
        for (int i = grabbedCards.Count - 1; i >= 0; i--)
        {
            this.PlacedCard(grabbedCards[i], grabbedCardBaseReceiver);
            grabbedCardBaseReceiver?.ForceLayCard(grabbedCards[i]);
        }

        ResetGrabbedCard();
    }

    private void ResetGrabbedCard()
    {
        for (int i = 0; i < grabbedCards.Count; i++)
        {
            grabbedCards[i].SpriteRenderer.sortingLayerName = SortingLayersNames.DEFAULT;
            grabbedCards[i].Trigger.enabled = true;
        }

        grabbedCards = new List<Card>();
        grabbedCardBaseReceiver = null;
    }
}
