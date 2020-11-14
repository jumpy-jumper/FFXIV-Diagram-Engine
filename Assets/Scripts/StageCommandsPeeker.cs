using UnityEngine;
using UnityEngine.UI;

public class StageCommandsPeeker : MonoBehaviour
{
    public Stage stage;
    public Color currentCommandColor;

    Text text;
    void Awake()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = "";

        foreach (IExecutable command in stage.commands)
        {
            if (stage.curCommand?.Value == command) text.text += $"<color=#{ColorUtility.ToHtmlStringRGBA(currentCommandColor)}>";
            text.text += command.GetType().ToString().Replace("Command", "") + "\n";
            if (stage.curCommand?.Value == command) text.text += "</color>";
        }
    }
}
