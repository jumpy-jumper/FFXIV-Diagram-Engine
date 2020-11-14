using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartSceneCommand : IExecutable
{
    public bool Execute(Stage stage)
    {
        stage.inputWait = true;
        stage.ReloadScene();
        return true;
    }

    public bool Reverse(Stage stage)
    {
        return false;
    }
}
