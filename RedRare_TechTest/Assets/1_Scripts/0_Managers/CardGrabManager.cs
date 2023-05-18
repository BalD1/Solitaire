using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGrabManager : EventHandlerMono
{
    private Card grabbedCard = null;

    private Camera cam = null;

    private Vector2 grabbedCardBasePos = Vector2.zero;

    private CardReceiver grabbedCardBaseReceiver = null;

    protected override void EventRegister()
    {
        InputManagerEventsHandler.OnClickableDown += OnClickDown;
        InputManagerEventsHandler.OnClickableUp += OnClickUp;
    }

    protected override void EventUnRegister()
    {
        InputManagerEventsHandler.OnClickableDown -= OnClickDown;
        InputManagerEventsHandler.OnClickableUp -= OnClickUp;
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
        //grabbedCard.transform.position = mousePos;
        grabbedCard.StartMovingTo(mousePos);
    }

    private void OnClickDown(IClickable clickable)
    {
        if (grabbedCard != null) return;

        CardReceiver cardReceiver = clickable.GetGameObject().GetComponent<CardReceiver>();
        if (cardReceiver == null) return;

        grabbedCard = cardReceiver.GetNextCard();
        grabbedCardBasePos = grabbedCard.transform.position;
        grabbedCardBaseReceiver = cardReceiver;

        if (grabbedCard != null)
            grabbedCard.SpriteRenderer.sortingLayerName = SortingLayersNames.FG_LOWEST;

    }

    private void OnClickUp(IClickable clickable)
    {
        if (grabbedCard == null) return;

        CardReceiver cardReceiver = clickable.GetGameObject().GetComponent<CardReceiver>();
        Debug.Log(cardReceiver);
        if (cardReceiver == null)
        {
            grabbedCard.StartMovingTo(grabbedCardBasePos, () => grabbedCardBaseReceiver.ForceLayCard(grabbedCard));
            grabbedCard = null;
            grabbedCardBaseReceiver = null;
            return;
        }

        grabbedCard.SpriteRenderer.sortingLayerName = SortingLayersNames.DEFAULT;
        cardReceiver.TryLayCard(grabbedCard);
        grabbedCard = null;
        grabbedCardBaseReceiver = null;
    }
}
