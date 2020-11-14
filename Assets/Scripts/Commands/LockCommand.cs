using System.Collections.Generic;
using UnityEngine;

public class LockCommand : IExecutable
{
    readonly string label;
    readonly string target;
    readonly Vector2 offset;

    public LockCommand(string label, string target, Vector2 offset)
    {
        this.label = label;
        this.target = target;
        this.offset = offset;
    }

    Dictionary<Actor, Vector2> originalPositions = new Dictionary<Actor, Vector2>();
    public bool Execute(Stage stage)
    {
        List<Actor> actors = stage.GetActors(label);
        GameObject obj = stage.actors.Find(a => a.name == target).gameObject;

        originalPositions.Clear();

        foreach (Actor actor in actors)
        {
            actor.GetComponent<PositionController>().Lock(obj, offset);
            originalPositions.Add(actor, actor.transform.position);
        }

        return true;
    }

    public bool Reverse(Stage stage)
    {
        foreach (Actor actor in originalPositions.Keys)
        {
            actor.GetComponent<PositionController>().Unlock();
            actor.transform.position = originalPositions[actor];
        }

        return true;
    }
}
