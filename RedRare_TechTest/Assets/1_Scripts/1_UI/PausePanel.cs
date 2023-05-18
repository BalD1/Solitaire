using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : EventHandlerMono
{
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private Button resumeButton;

    private bool isActive = false;
    private bool isTweening = false;

    private void Reset()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();

        foreach (Button item in this.transform.GetComponentsInChildren<Button>())
        {
            if (item.gameObject.name.Contains("Resume")) resumeButton = item;
        }
    }

    protected override void Start()
    {
        base.Start();

        resumeButton.onClick.AddListener(SetState);
    }

    protected override void EventRegister()
    {
        InputManagerEventsHandler.OnPause += SetState;
    }

    protected override void EventUnRegister()
    {
        InputManagerEventsHandler.OnPause -= SetState;
    }

    public void SetState()
    {
        if (isTweening)
        {
            LeanTween.cancel(canvasGroup.gameObject);
            canvasGroup.alpha = isActive ? 1 : 0;

            OnLeanEnded();
        }

        isActive = !isActive;

        isTweening = true;

        canvasGroup.LeanAlpha(isActive ? 1 : 0, .25f).setOnComplete(OnLeanEnded);
    }

    private void OnLeanEnded()
    {
        isTweening = false;

        canvasGroup.interactable = isActive ? true : false;
        canvasGroup.blocksRaycasts = isActive ? true : false;
    }
}
