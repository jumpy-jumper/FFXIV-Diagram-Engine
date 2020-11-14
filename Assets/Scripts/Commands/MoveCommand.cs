using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : IExecutable
{
    readonly string label;
    readonly PositionController.MovementType movementType;
    readonly float timeFactor;
    readonly Vector2 target;

    public MoveCommand(string label, PositionController.MovementType movementType, float timeFactor, Vector2 target)
    {
        this.label = label;
        this.movementType = movementType;
        this.timeFactor = timeFactor;
        this.target = target;
    }

    Dictionary<PositionController, Vector2> originalPositions = new Dictionary<PositionController, Vector2>();
    public bool Execute(Stage stage)
    {                                                                 
        List<Actor> actors = stage.GetActors(label);

        foreach (Actor actor in actors)
        {
            PositionController moveable = actor.GetComponent<PositionController>();
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
        foreach (PositionController moveable in originalPositions.Keys)
        {
            moveable.Move(PositionController.MovementType.Instant, 0, originalPositions[moveable]);
        }
        originalPositions.Clear();
        return true;
    }
}
