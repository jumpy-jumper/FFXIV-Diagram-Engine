using System.Collections.Generic;
using UnityEngine;

public class DespawnCommand : IExecutable
{
    readonly string label;

    public DespawnCommand(string label)
    {
        this.label = label;
    }

    List<Actor> targets = new List<Actor>();

    public bool Execute(Stage stage)
    {
        targets = stage.GetActors(label);

        foreach (Actor actor in targets)
        {
            actor.gameObject.SetActive(false);
            stage.actors.Remove(actor);
        }

        return true;
    }

    public bool Reverse(Stage stage)
    {
        foreach (Actor actor in targets)
        {
            actor.gameObject.SetActive(true);
            stage.actors.Add(actor);
        }

        return true;
    }
}
