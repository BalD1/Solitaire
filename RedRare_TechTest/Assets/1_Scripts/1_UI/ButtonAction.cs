using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    public void ChangeScene(ButtonArgs_Scene buttonArgs) => BuiltScenesManager.ChangeScene(buttonArgs.GetArgs);

    public void ReloadScene() => BuiltScenesManager.ReloadScene();

    public void Quit() => Application.Quit();
}
