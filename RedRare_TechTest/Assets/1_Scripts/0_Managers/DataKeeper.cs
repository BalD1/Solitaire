
public class DataKeeper
{
	public static LocalizationManager.Languages CurrentLanguage = LocalizationManager.Languages.EN;

	public static ProfilePanel.ProfilesData CurrentProfile = new ProfilePanel.ProfilesData();

	public static bool HaveValidProfile() => !CurrentProfile.Equals(default(ProfilePanel.ProfilesData));
}