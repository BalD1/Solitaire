using UnityEngine;

[RequireComponent(typeof(CanvasGroupController))]
public class WinPanel : EventHandlerMono
{
    [SerializeField] private CanvasGroupController groupController;

    private void Reset()
    {
        groupController = this.GetComponent<CanvasGroupController>();
    }

    protected override void EventRegister()
    {
        SolitaireManagerEventsHandler.OnWin += OnWin;
    }

    protected override void EventUnRegister()
    {
        SolitaireManagerEventsHandler.OnWin -= OnWin;
    }

    private void OnWin()
    {
        groupController.Show();
    }
}
