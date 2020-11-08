using UnityEngine;

/*
 * Orders the level to wait for user input.
 */
public class WaitForInputCommand : IExecutable
{
    public bool Execute(Level level)
    {
        level.inputWait = true;
        return true;
    }
}
