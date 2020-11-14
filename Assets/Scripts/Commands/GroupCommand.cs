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

    public bool Execute(Level level)
    {
        List<Actor> actors = level.GetActors(names);
        level.groups.Add(label, actors);
        return true;
    }
}
