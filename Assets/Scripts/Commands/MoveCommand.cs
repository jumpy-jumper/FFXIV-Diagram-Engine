using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : IExecutable
{
    readonly string label;
    readonly Moveable.MovementType movementType;
    readonly float timeFactor;
    readonly Vector2 target;

    public MoveCommand(string label, Moveable.MovementType movementType, float timeFactor, Vector2 target)
    {
        this.label = label;
        this.movementType = movementType;
        this.timeFactor = timeFactor;
        this.target = target;
    }

    public bool Execute(Level level)
    {                                                                 
        List<Actor> actors = level.GetActors(label);

        foreach (Actor actor in actors)
        {
            Moveable moveableComponent = actor.GetComponent<Moveable>();
            if (moveableComponent)
            {
                moveableComponent.Move(movementType, timeFactor, target);
            }
        }

        return true;
    }
}
