using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class LocalizationManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    public enum Languages
    {
        EN,
        FR,
    }

    private void Reset()
    {
        dropdown = this.GetComponent<TMP_Dropdown>();
    }

    private void Awake()
    {
        dropdown.onValueChanged.AddListener(OnDropDownValueChanged);
    }

    private void Start()
    {
        CheckLanguage();
    }

    /// <summary>
    /// Checks the language setting at the start of the scene
    /// </summary>
    private void CheckLanguage()
    {
        Languages savedLanguage = (Languages)SaveManager.GetSavedIntKey(SaveManager.E_SaveKeys.I_Language);
        this.LanguageChanged(savedLanguage);
        dropdown.SetValueWithoutNotify((int)savedLanguage);
    }

    /// <summary>
    /// When the dropdown value of the options panel changes
    /// </summary>
    /// <param name="val"></param>
    private void OnDropDownValueChanged(int val)
    {
        Languages newLanguage = (Languages)val;
        this.LanguageChanged(newLanguage);
        DataKeeper.CurrentLanguage = newLanguage;

        SaveManager.SetSavedKey(SaveManager.E_SaveKeys.I_Language, (int)newLanguage);
    }
}
