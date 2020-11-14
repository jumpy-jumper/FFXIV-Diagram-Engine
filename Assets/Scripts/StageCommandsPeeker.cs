using UnityEngine;
using UnityEngine.UI;

public class StageCommandsPeeker : MonoBehaviour
{
    public Stage stage;
    public enum WhatToPrint { Queue, History }
    public WhatToPrint whatToPrint;

    Text text;
    void Awake()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        switch (whatToPrint)
        {
            case WhatToPrint.Queue:
                text.text = "<b>Command Stack</b>\n\n";
                foreach (IExecutable command in stage.commands)
                {
                    text.text += command.GetType().ToString() + "\n";
                    if (command.GetType() == typeof(WaitForInputCommand))
                    {
                        text.text += "\n";
                    }
                }
                break;
            case WhatToPrint.History:
                text.text = "<b>Command History</b>\n\n";
                foreach (IExecutable command in stage.history)
                {
                    if (command.GetType() == typeof(WaitForInputCommand))
                    {
                        text.text += "\n";
                    }
                    text.text += command.GetType().ToString() + "\n";
                }
                break;
        }
    }
}
