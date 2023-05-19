using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class AudioSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    [SerializeField] private SaveManager.E_SaveKeys saveKey;

    [SerializeField] private float volMultiplier = 30f;

    [SerializeField] private AudioMixer mainMixer;

    private void Reset()
    {
        slider = this.GetComponent<Slider>();
    }

    private void Start()
    {
        LoadSlidersValue();
    }

    /// <summary>
    /// Loads the sliders value from <seealso cref="SaveManager.GetSavedFloatKey(SaveManager.E_SaveKeys)"/>
    /// </summary>
    private void LoadSlidersValue()
    {
        slider.value = SaveManager.GetSavedFloatKey(saveKey);
    }

    public void OnMainSliderValueChange(float value) => HandleSliderChange(value, saveKey);

    /// <summary>
    /// Changes the <paramref name="param"/> key in the <seealso cref="mainMixer"/> with <paramref name="value"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="param"></param>
    /// <param name="key"></param>
    private void HandleSliderChange(float value, SaveManager.E_SaveKeys key)
    {
        float newVol = 0;
        if (value > 0) newVol = Mathf.Log10(value) * volMultiplier;
        else newVol = -80f;

        string param = "";
        switch (key)
        {
            case SaveManager.E_SaveKeys.F_MasterVolume:
                param = AudioManager.masterVolParam;
                break;

            case SaveManager.E_SaveKeys.F_MusicVolume:
                param = AudioManager.musicVolParam;
                break;

            case SaveManager.E_SaveKeys.F_SFXVolume:
                param = AudioManager.sfxVolparam;
                break;
        }

        mainMixer.SetFloat(param, newVol);
        SaveManager.SetSavedKey(key, value);
    }
}
