using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button validateNewProfileButton;

    [SerializeField] private TMP_Dropdown profilesDropdown;

    [SerializeField] private TextMeshProUGUI playedGamesCountTXT;
    [SerializeField] private TextMeshProUGUI fastestGameTXT;

    [InspectorButton(nameof(SaveProfilesToFile))]
    [SerializeField] private bool saveProfilesToFile;

    private string inputFieldResult = "";

    [System.Serializable]
    private struct ProfilesData
    {
        public string name;
        public int playedGames;
        public float fastestGame;

        public ProfilesData(string _name,  int _playedGames, float _fastestGame)
        {
            name = _name;
            playedGames = _playedGames;
            fastestGame = _fastestGame;
        }
    }

    private List<ProfilesData> profiles = new List<ProfilesData>();

    private void Reset()
    {
        inputField = this.GetComponentInChildren<TMP_InputField>();
        profilesDropdown = this.GetComponentInChildren<TMP_Dropdown>();
    }

    private void Awake()
    {
        profiles = LoadProfilesFromFile();

        Application.quitting += SaveProfilesToFile;

        PopulateProfileDropdown();
        inputField?.onEndEdit.AddListener(OnInputFieldEndedit);

        if (validateNewProfileButton != null )
        {
            validateNewProfileButton.onClick.AddListener(ValidateNewProfile);
            validateNewProfileButton.interactable = false;
        }

        profilesDropdown?.onValueChanged.AddListener(ShowData);
    }

    private void PopulateProfileDropdown()
    {
        List<string> profilesNames = new List<string>();
        foreach (var item in profiles)
            profilesNames.Add(item.name);

        profilesDropdown.AddOptions(profilesNames);

        ShowData(0);
    }

    private void ShowData(int id)
    {
        if (profiles.Count == 0) return;

        playedGamesCountTXT.text = profiles[id].playedGames.ToString();
        fastestGameTXT.text = profiles[id].fastestGame.ToString();
    }

    private void OnInputFieldEndedit(string result)
    {
        inputFieldResult = result;
        if (result != "") validateNewProfileButton.interactable = true;
    }

    private void ValidateNewProfile()
    {
        ProfilesData newProfile = new ProfilesData(inputFieldResult, 0, 0);
        profiles.Add(newProfile);

        TMPro.TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(newProfile.name);
        profilesDropdown.options.Add(optionData);

        if (profiles.Count == 1)
        {
            profilesDropdown.SetValueWithoutNotify(0);
            ShowData(0);
        }
    }

    private void SaveProfilesToFile()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/profiles.prf";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, profiles);
        stream.Close();
    }

    private List<ProfilesData> LoadProfilesFromFile()
    {
        string path = Application.persistentDataPath + "/profiles.prf";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            List<ProfilesData> profiles = formatter.Deserialize(stream) as List<ProfilesData>;
            stream.Close();

            return profiles;
        }
        else return null;
    }
}
