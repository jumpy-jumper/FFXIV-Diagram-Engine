using UnityEngine;

/*
 * Orders the stage to wait for user input.
 */
public class WaitForInputCommand : IExecutable
{
    public bool Execute(Stage stage)
    {
        stage.inputWait = true;
        return true;
    }

    public bool Reverse(Stage stage)
    {
        return true;
    }
}
