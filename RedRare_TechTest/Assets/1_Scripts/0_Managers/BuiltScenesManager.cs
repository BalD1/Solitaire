using UnityEngine.SceneManagement;

public static class BuiltScenesManager
{
    public enum E_ScenesNames
    {
        MainMenu,
        MainScene,
    }

    /// <summary>
    /// Loads the <paramref name="newScene"/> scene.
    /// </summary>
    /// <param name="newScene"></param>
    public static void ChangeScene(E_ScenesNames newScene)
    {
        SceneManager.LoadScene(newScene.ToString());
    }

    /// <summary>
    /// Completly reloads the currently active scene.
    /// </summary>
    public static void ReloadScene() => 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}