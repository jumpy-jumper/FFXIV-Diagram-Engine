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

    Dictionary<Moveable, Vector2> originalPositions = new Dictionary<Moveable, Vector2>();
    public bool Execute(Stage stage)
    {                                                                 
        List<Actor> actors = stage.GetActors(label);

        foreach (Actor actor in actors)
        {
            Moveable moveable = actor.GetComponent<Moveable>();
            if (moveable)
            {
                originalPositions.Add(moveable, moveable.transform.position);
                moveable.Move(movementType, timeFactor, target);
            }
        }

        return true;
    }

    public bool Reverse(Stage stage)
    {
        foreach (Moveable moveable in originalPositions.Keys)
        {
            moveable.Move(Moveable.MovementType.Instant, 0, originalPositions[moveable]);
        }
        originalPositions.Clear();
        return true;
    }
}
