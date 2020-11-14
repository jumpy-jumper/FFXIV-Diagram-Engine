using UnityEngine;

public class Moveable : MonoBehaviour
{
    public enum MovementType { Instant, Speed, Duration }
    public void Move(MovementType movementType, float timeFactor, Vector2 target)
    {
        switch (movementType)
        {
            case MovementType.Instant:
                transform.position = target;
                break;
        }
    }

    public bool IsMoving { get => false; }
}
