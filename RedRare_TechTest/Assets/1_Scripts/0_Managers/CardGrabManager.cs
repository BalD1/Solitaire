using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGrabManager : EventHandlerMono
{
    private Card grabbedCard = null;

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
        if (grabbedCard != null) MoveGrabbedCardToMouse();
    }

    private void MoveGrabbedCardToMouse()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        grabbedCard.StartMovingTo(mousePos);
    }

    private void OnClickDown(IClickable clickable)
    {
        if (grabbedCard != null) return;

        grabbedCard = clickable.GetGameObject().GetComponent<Card>();
        if (grabbedCard == null) return;

        grabbedCardBaseReceiver = grabbedCard.GetCardReceiver();
        grabbedCardBaseReceiver.GetNextCard();

        grabbedCard.Trigger.enabled = false;

        grabbedCard.SpriteRenderer.sortingLayerName = SortingLayersNames.FG_LOWEST;

    }

    private void OnClickabkleUp(IClickable clickable)
    {
        if (grabbedCard == null) return;

        Card card = clickable.GetGameObject().GetComponent<Card>();

        CardReceiver cardReceiver = card != null ? card.GetCardReceiver() :
                                    clickable.GetGameObject().GetComponent<CardReceiver>();
        if (cardReceiver == null) return;

        if (!cardReceiver.TryLayCard(grabbedCard)) return;

        this.PlacedCard(grabbedCard, cardReceiver);

        ResetGrabbedCard();
    }

    private void OnMouseInputUp(Vector2 mousePos)
    {
        if (grabbedCard == null) return;

        this.PlacedCard(grabbedCard, grabbedCardBaseReceiver);
        grabbedCardBaseReceiver.ForceLayCard(grabbedCard);

        ResetGrabbedCard();
    }

    private void ResetGrabbedCard()
    {
        grabbedCard.SpriteRenderer.sortingLayerName = SortingLayersNames.DEFAULT;
        grabbedCard.Trigger.enabled = true;
        grabbedCard = null;
        grabbedCardBaseReceiver = null;
    }
}
