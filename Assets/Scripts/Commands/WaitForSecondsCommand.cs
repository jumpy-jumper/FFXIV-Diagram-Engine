using UnityEngine;

public class WaitForSecondsCommand : IExecutable
{
    public float seconds;

    public WaitForSecondsCommand()
    {
        seconds = 0;
    }

    public bool Execute(Level level)
    {
        if (seconds == 0)
        {
            Debug.LogError("Wait for seconds Command: No time specified");
            return false;
        }

        level.secondsWait = seconds;
        return true;
    }
}
