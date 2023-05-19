using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        if (grabbedCards.Count > 0) return;

        Card clickedOnCard = clickable.GetGameObject().GetComponent<Card>();
        if (clickedOnCard == null) return;

        grabbedCardBaseReceiver = clickedOnCard.GetCardReceiver();
        if (grabbedCardBaseReceiver != null)
            grabbedCards = grabbedCardBaseReceiver.GetEveryCardsTo(clickedOnCard);
        else
            grabbedCards = new List<Card>() { clickedOnCard};

        foreach (var item in grabbedCards)
        {
            item.Trigger.enabled = false;
            item.SpriteRenderer.sortingLayerName = SortingLayersNames.FG_LOWEST;
        }

    }

    private void OnClickabkleUp(IClickable clickable)
    {
        if (grabbedCards.Count == 0) return;

        Card card = clickable.GetGameObject().GetComponent<Card>();

        CardReceiver cardReceiver = card != null ? card.GetCardReceiver() :
                                    clickable.GetGameObject().GetComponent<CardReceiver>();
        if (cardReceiver == null) return;

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
