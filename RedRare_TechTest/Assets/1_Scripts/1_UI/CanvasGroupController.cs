using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupController : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;

    [SerializeField] private float fadeDuration = .25f;

    private void Reset()
    {
        group = this.GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        group.LeanAlpha(1, fadeDuration).setOnComplete(() => SetInteractable(true));
    }

    public void Hide()
    {
        group.LeanAlpha(0, fadeDuration).setOnComplete(() => SetInteractable(false));
    }

    private void SetInteractable(bool interactable)
    {
        group.interactable = interactable;
        group.blocksRaycasts = interactable;
    }
}
