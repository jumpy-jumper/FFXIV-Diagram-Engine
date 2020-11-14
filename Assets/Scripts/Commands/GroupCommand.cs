using System.Collections.Generic;
using UnityEngine;

/*
 * Create a new group of actors.
 */
public class GroupCommand : IExecutable
{
    readonly string label;
    readonly List<string> names;

    public GroupCommand(string label, List<string> names)
    {
        this.label = label;
        this.names = names;
    }

    public bool Execute(Stage stage)
    {
        List<Actor> actors = stage.GetActors(names);
        stage.groups.Add(label, actors);
        return true;
    }

    public bool Reverse(Stage stage)
    {
        stage.groups.Remove(label);
        return true;
    }
}
