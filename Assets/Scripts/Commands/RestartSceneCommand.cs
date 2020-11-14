using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartSceneCommand : IExecutable
{
    public bool Execute(Stage stage)
    {
        stage.inputWait = true; // will prevent the stage from attempting to execute the next command
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        return true;
    }

    public bool Reverse(Stage stage)
    {
        return false;
    }
}
