using System.Collections.Generic;
using UnityEngine;

/*
 * Create a new group of actors.
 */
public class GroupCommand : IExecutable
{
    public string label;
    public List<string> names;

    public GroupCommand()
    {
        label = "";
        names = new List<string>();
    }

    public bool Execute(Level level)
    {
        // Check the label is valid.
        if (label == "")
        {
            Debug.LogError("Group Command: No label specified");
            return false;
        }
        if (level.IsNameConflict(label))
        {
            Debug.LogError("Group Command: Name conflict: \"" + label + "\"");
            return false;
        }

        // Check the list of names is not empty
        if (names.Count == 0)
        {
            Debug.LogError("Group Command: No actors specified");
            return false;
        }

        // Build the actors list and add to level's dictionary

        List<Actor> actors = level.GetActors(names);
        if (actors == null)
        {
            Debug.LogError("Group Command: Level returned null list");
            return false;
        }

        level.groups.Add(label, actors);
        return true;
    }
}
