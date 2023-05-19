using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void CheckLanguage()
    {
        Languages savedLanguage = (Languages)SaveManager.GetSavedIntKey(SaveManager.E_SaveKeys.I_Language);
        this.LanguageChanged(savedLanguage);
        dropdown.SetValueWithoutNotify((int)savedLanguage);
    }

    private void OnDropDownValueChanged(int val)
    {
        Languages newLanguage = (Languages)val;
        this.LanguageChanged(newLanguage);
        DataKeeper.CurrentLanguage = newLanguage;

        SaveManager.SetSavedKey(SaveManager.E_SaveKeys.I_Language, (int)newLanguage);
    }
}
