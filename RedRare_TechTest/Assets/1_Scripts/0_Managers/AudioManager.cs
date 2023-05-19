using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using BalDUtilities.Misc;
using UnityEngine.SceneManagement;
using System;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfx2DSource;
    [SerializeField] private AudioSource[] soundsSource;

    [SerializeField] private AudioMixer mainMixer;

    public const string masterVolParam = "MasterVol";
    public const string musicVolParam = "MusicVol";
    public const string sfxVolparam = "SFXVol";

    public const string masterPitchParam = "MasterPitch";
    public const string musicPitchParam = "MusicPitch";
    public const string sfxPitchParam = "SFXPitch";

    private const float baseVolumeValue = .5f;

    [Space]
    [SerializeField] private float volMultiplier = 30f;

    [SerializeField] private float musicFadeSpeed = 1f;


    [System.Serializable]
    public enum E_SFXClipsTags
    {
        Clic,
    }

    [System.Serializable]
    public enum E_MusicClipsTags
    {
        MainMenu,
        MainScene,
        BossMusic,
        InLobby,
    }

    [System.Serializable]
    public struct MusicClips
    {
#if UNITY_EDITOR
        public string inEditorName;
#endif
        public E_MusicClipsTags tag;
        public AudioClip clip;
    }

    [System.Serializable]
    public struct SFXClips
    {
#if UNITY_EDITOR
        public string inEditorName;
#endif
        public E_SFXClipsTags tag;
        public AudioClip clip;
    }

    [SerializeField] private MusicClips[] musicClipsByTag;
    [SerializeField] private SFXClips[] sfxClipsByTag;

    private void Start()
    {
        LoadParams();

        ChangeMainMixerPitch(1);
        ChangeMusicMixerPitch(1);
        ChangeSFXMixerPitch(1);
    }

    private void LoadParams()
    {
        string masterKey = SaveManager.E_SaveKeys.F_MasterVolume.ToString();
        string musicKey = SaveManager.E_SaveKeys.F_MusicVolume.ToString();
        string sfxKey = SaveManager.E_SaveKeys.F_SFXVolume.ToString();

        GetParam(masterKey, masterVolParam);
        GetParam(musicKey, musicVolParam);
        GetParam(sfxKey, sfxVolparam);

        mainMixer.SetFloat(musicVolParam, SaveManager.GetSavedFloatKey(SaveManager.E_SaveKeys.F_MusicVolume));
        mainMixer.SetFloat(sfxVolparam, SaveManager.GetSavedFloatKey(SaveManager.E_SaveKeys.F_SFXVolume));
    }

    private void GetParam(string key, string param)
    {
        if (!PlayerPrefs.HasKey(key)) PlayerPrefs.SetFloat(key, baseVolumeValue);
        mainMixer.SetFloat(param, SaveManager.GetSavedFloatKey(key));
    }

    public void ChangeMixerPitch(string param, float newPitch) => mainMixer.SetFloat(param, newPitch);
    public void ChangeMainMixerPitch(float newPitch) => ChangeMixerPitch(masterPitchParam, newPitch);
    public void ChangeMusicMixerPitch(float newPitch) => ChangeMixerPitch(musicPitchParam, newPitch);
    public void ChangeSFXMixerPitch(float newPitch) => ChangeMixerPitch(sfxPitchParam, newPitch);

    /// <summary>
    /// Plays the sound with <paramref name="key"/> as tag, stored in <seealso cref="sfxClipsByTag"/>.
    /// </summary>
    /// <param name="key"></param>
    public void Play2DSFX(E_SFXClipsTags key)
    {
        foreach (var item in sfxClipsByTag)
        {
            if (item.tag.Equals(key))
            {
                sfx2DSource.PlayOneShot(item.clip);
                return;
            }
        }

        Debug.LogError("Could not find " + EnumsExtension.EnumToString(key) + " in sfxClipsByTag");
    }

    /// <summary>
    /// Plays the sound with <paramref name="key"/> as tag, stored in <seealso cref="sfxClipsByTag"/>.
    /// </summary>
    /// <param name="key"></param>
    public void Play2DSFX(string key)
    {
        foreach (var item in sfxClipsByTag)
        {
            if (EnumsExtension.EnumToString(item.tag).Equals(key))
            {
                sfx2DSource.PlayOneShot(item.clip);
                return;
            }
        }

        Debug.LogError("Could not find " + key + " in sfxClipsByTag");
    }

    public void PauseMusic() => FadeMusic(true, () => musicSource.Pause());
    public void ResumeMusic() => PlayActionAndFadeMusic(false, () => musicSource.UnPause());
    public void StopMusic() => FadeMusic(true, () => musicSource.Stop());
    public void PlayMusic(E_MusicClipsTags musicTag)
    {
        AudioClip musicToPlay = null;

        foreach (var item in musicClipsByTag)
        {
            if (item.tag.Equals(musicTag))
            {
                musicToPlay = item.clip;
                break;
            }
        }

        if (musicSource.isPlaying) FadeMusic(true, () =>
        {
            musicSource.clip = musicToPlay;
            FadeMusic(false);
        });
        else
        {
            musicSource.clip = musicToPlay;
            FadeMusic(false);
        }
    }

    private void FadeMusic(bool fadeOut, Action onComplete = null)
    {
        if (!fadeOut) musicSource.Play();
        LeanTween.value(musicSource.volume, fadeOut ? 0 : 1, musicFadeSpeed).setIgnoreTimeScale(true).setOnUpdate((float val) =>
        {
            musicSource.volume = val;
        }).setOnComplete(onComplete);
    }
    private void PlayActionAndFadeMusic(bool fadeOut, Action beforeAction)
    {
        beforeAction?.Invoke();
        LeanTween.value(musicSource.volume, fadeOut ? 0 : 1, musicFadeSpeed).setIgnoreTimeScale(true).setOnUpdate((float val) =>
        {
            musicSource.volume = val;
        });
    }
}
