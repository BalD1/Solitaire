using System;
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

    [SerializeField] private CanvasGroupController createProfileCTRL;
    [SerializeField] private CanvasGroupController profileStatsCTRL;

    [InspectorButton(nameof(SaveProfilesToFile), ButtonWidth = 200)]
    [SerializeField] private bool saveProfilesToFile;

    [InspectorButton(nameof(WipeOutSaves), ButtonWidth = 200)]
    [SerializeField] private bool wipeOutSaves;

    private string inputFieldResult = "";

    [System.Serializable]
    public struct ProfilesData
    {
        public string uniqueID;
        public string name;
        public int playedGames;
        public float fastestGame;

        public ProfilesData(string _uniqueID, string _name,  int _playedGames, float _fastestGame)
        {
            uniqueID = _uniqueID;
            name = _name;
            playedGames = _playedGames;
            fastestGame = _fastestGame;
        }
    }

    [Header("Profiles list, careful with edition")]
    [SerializeField] private List<ProfilesData> profiles = new List<ProfilesData>();

    private void Reset()
    {
        inputField = this.GetComponentInChildren<TMP_InputField>();
        profilesDropdown = this.GetComponentInChildren<TMP_Dropdown>();
    }

    private void Awake()
    {
        profiles = LoadProfilesFromFile();

        PopulateProfileDropdown();
        InitializeProfilesDataOnUI();

        Application.quitting += SaveProfilesToFile;

        // setup listeners on UI
        inputField?.onEndEdit.AddListener(OnInputFieldEndedit);

        if (validateNewProfileButton != null)
        {
            validateNewProfileButton.onClick.AddListener(ValidateNewProfile);
            validateNewProfileButton.interactable = false;
        }

        if (profilesDropdown != null)
        {
            profilesDropdown.onValueChanged.AddListener(ShowData);
            profilesDropdown.onValueChanged.AddListener(SetCurrentProfile);
        }
    }

    private void InitializeProfilesDataOnUI()
    {
        // if there is no profile, show "create profile" prompt
        if (profiles.Count == 0)
        {
            createProfileCTRL.Show();
            profileStatsCTRL.Hide();
            return;
        }
        // else, show stats 

        createProfileCTRL.Hide();
        profileStatsCTRL.Show();

        // set a profile in datakeeper if there is none
        if (!DataKeeper.HaveValidProfile())
        {
            SetCurrentProfile(0);
            return;
        }
        //else show the profile in datakeeper
        
        for (int i = 0; i < profiles.Count; i++)
        {
            if (profiles[i].uniqueID == DataKeeper.CurrentProfile.uniqueID)
            {
                profiles[i] = DataKeeper.CurrentProfile;
                SetDropdownTextOnElement(i);
                ShowData(i);
                break;
            }
        }
        
    }

    private void PopulateProfileDropdown()
    {
        List<string> profilesNames = new List<string>();
        foreach (var item in profiles)
            profilesNames.Add(item.name);

        profilesDropdown.AddOptions(profilesNames);

        ShowData(0);
    }

    /// <summary>
    /// Shows the profile's stats
    /// </summary>
    /// <param name="id"></param>
    private void ShowData(int id)
    {
        if (profiles.Count == 0) return;

        playedGamesCountTXT.text = profiles[id].playedGames.ToString();

        float fastestGameTime = profiles[id].fastestGame;

        if (fastestGameTime == -1)
            fastestGameTXT.text = "-";
        else
        {
            TimeSpan time = TimeSpan.FromSeconds(fastestGameTime);

            fastestGameTXT.text = time.ToString(@"hh\:mm\:ss");
        }
    }
    private void SetCurrentProfile(int id)
    {
        DataKeeper.CurrentProfile = profiles[id];
    }

    private void OnInputFieldEndedit(string result)
    {
        inputFieldResult = result;
        if (result != "") validateNewProfileButton.interactable = true;
    }

    /// <summary>
    /// When the validate button for a new profile is pressed
    /// </summary>
    private void ValidateNewProfile()
    {
        // create new profile
        ProfilesData newProfile = new ProfilesData(Guid.NewGuid().ToString(), inputFieldResult, 0, -1);
        profiles.Add(newProfile);

        // add it to dropdown
        TMPro.TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(newProfile.name);
        profilesDropdown.options.Add(optionData);

        // show it on UI
        if (profiles.Count == 1)
        {
            SetDropdownTextOnElement(0);
            profileStatsCTRL.Show();
            createProfileCTRL.Hide();
        }

        int profileIdx = profiles.Count - 1;
        ShowData(profileIdx);
        SetCurrentProfile(profileIdx);
        SetDropdownTextOnElement(profileIdx);
    }

    private void SetDropdownTextOnElement(int elementID)
    {
        profilesDropdown.SetValueWithoutNotify(elementID);
        profilesDropdown.captionText.text = profiles[elementID].name;
    }

    public void SaveProfilesToFile()
    {
        SaveProfilesToFile(profiles);
    }
    public static void SaveProfilesToFile(List<ProfilesData> profiles)
    {
        if (profiles == null) profiles = new List<ProfilesData>();
        
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/profiles.prf";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, profiles);
        stream.Close();
    }

    public static List<ProfilesData> LoadProfilesFromFile()
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

    private void WipeOutSaves()
    {
        SaveProfilesToFile(new List<ProfilesData>());
    }
}
