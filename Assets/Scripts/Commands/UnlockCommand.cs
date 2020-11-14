using System.Collections.Generic;
using UnityEngine;

public class UnlockCommand : IExecutable
{
    readonly string label;

    public UnlockCommand(string label)
    {
        this.label = label;
    }

    struct LockData
    {
        public readonly GameObject target;
        public readonly Vector2 offset;

        public LockData(GameObject target, Vector2 offset)
        {
            this.target = target;
            this.offset = offset;
        }
    }

    Dictionary<Actor, LockData> originalLocks = new Dictionary<Actor, LockData>();
    public bool Execute(Stage stage)
    {
        List<Actor> actors = stage.GetActors(label);

        originalLocks.Clear();
        foreach (Actor actor in actors)
        {
            PositionController ctrl = actor.GetComponent<PositionController>();
            originalLocks.Add(actor, new LockData(ctrl.parent, ctrl.offset));
            ctrl.Unlock();
        }

        return true;
    }

    public bool Reverse(Stage stage)
    {
        foreach (Actor actor in originalLocks.Keys)
        {
            actor.GetComponent<PositionController>().Lock(originalLocks[actor].target, originalLocks[actor].offset);
        }
        return true;
    }
}
