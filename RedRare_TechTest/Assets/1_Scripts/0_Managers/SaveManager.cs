using UnityEngine;

public static class SaveManager
{
    public enum E_SaveKeys
    {
        // floats

        F_MasterVolume,
        F_MusicVolume,
        F_SFXVolume,

        // ints

        I_Language,

        // bools

        // strings
    }

    /*
     * SETTERS
     * Les SetSavedKey sauvegardent la valeur donnée avec "value" avec une clé "key"
     */

    public static void SetSavedKey(E_SaveKeys key, int value) => PlayerPrefs.SetInt(key.ToString(), value);
    public static void SetSavedKey(E_SaveKeys key, bool value)
    {
        int ival = value ? 1 : 0;
        PlayerPrefs.SetInt(key.ToString(), ival);
    }
    public static void SetSavedKey(E_SaveKeys key, float value) => PlayerPrefs.SetFloat(key.ToString(), value);
    public static void SetSavedKey(E_SaveKeys key, string value) => PlayerPrefs.SetString(key.ToString(), value);

    /*
    * GETTERS
    * Les GetSavedKey retournent la valeur sauvegardée avec la clé "key"
    */

    public static int GetSavedIntKey(E_SaveKeys key) => PlayerPrefs.GetInt(key.ToString());
    public static int GetSavedIntKey(string key) => PlayerPrefs.GetInt(key);

    public static bool GetSavedBoolKey(E_SaveKeys key) => PlayerPrefs.GetInt(key.ToString()) == 1 ? true : false;
    public static bool GetSavedBoolKey(string key) => PlayerPrefs.GetInt(key) == 1 ? true : false;

    public static float GetSavedFloatKey(E_SaveKeys key) => PlayerPrefs.GetFloat(key.ToString());
    public static float GetSavedFloatKey(string key) => PlayerPrefs.GetFloat(key);

    public static string GetSavedStringKey(E_SaveKeys key) => PlayerPrefs.GetString(key.ToString());
    public static string GetSavedStringKey(string key) => PlayerPrefs.GetString(key);

    public static void DeleteKey(E_SaveKeys key) => PlayerPrefs.DeleteKey(key.ToString());
}
