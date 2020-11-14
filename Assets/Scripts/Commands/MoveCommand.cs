using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : IExecutable
{
    public string label;
    public Moveable.MovementType movementType;
    public float timeFactor;
    public Vector2 target;

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
