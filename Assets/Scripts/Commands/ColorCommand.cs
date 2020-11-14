using System.Collections.Generic;
using UnityEngine;

public class ColorCommand : IExecutable
{
    readonly string label;
    readonly Color color;

    public ColorCommand(string label, Color color)
    {
        this.label = label;
        this.color = color;
    }

    Dictionary<Actor, Color> originalColors = new Dictionary<Actor, Color>();
    public bool Execute(Stage stage)
    {
        originalColors.Clear();
        List<Actor> actors = stage.GetActors(label);

        foreach (Actor actor in actors)
        {
            SpriteRenderer spr = actor.GetComponent<SpriteRenderer>();
            originalColors.Add(actor, spr.color);
            Color newColor = color;
            newColor.a = spr.color.a;
            spr.color = newColor;
        }

        return true;
    }

    public bool Reverse(Stage stage)
    {
        foreach (Actor actor in originalColors.Keys)
        {
            actor.GetComponent<SpriteRenderer>().color = originalColors[actor];
        }

        return true;
    }
}
