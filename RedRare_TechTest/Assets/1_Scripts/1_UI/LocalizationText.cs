using TMPro;
using UnityEngine;

public class LocalizationText : EventHandlerMono
{
    [System.Serializable]
    private struct TextTraductions
    {
        public LocalizationManager.Languages language;
        public string text;
    }

    [SerializeField] private TextMeshProUGUI textMesh;

    [SerializeField] private TextTraductions[] textTraductions;

    private void Reset()
    {
        textMesh = this.GetComponent<TextMeshProUGUI>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void EventRegister()
    {
        LocalizationManagerEventsHandler.OnLanguageChanged += Traduct;
    }

    protected override void EventUnRegister()
    {
        LocalizationManagerEventsHandler.OnLanguageChanged -= Traduct;
    }

    private void Traduct(LocalizationManager.Languages language)
    {
        foreach (var item in textTraductions)
        {
            if (item.language != language) continue;
            textMesh.text = item.text;

            return;
        }
    }
}
