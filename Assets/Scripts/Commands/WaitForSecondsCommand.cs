using UnityEngine;

public class WaitForSecondsCommand : IExecutable
{
    public float seconds;

    public WaitForSecondsCommand()
    {
        seconds = 0;
    }

    public bool Execute(Stage stage)
    {
        if (seconds == 0)
        {
            Debug.LogError("Wait for seconds Command: No time specified");
            return false;
        }

        stage.secondsWait = seconds;
        return true;
    }

    public bool Reverse(Stage stage)
    {
        return true;
    }
}
