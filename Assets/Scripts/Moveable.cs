using UnityEngine;

public class Moveable : MonoBehaviour
{
    bool isMoving;

    float speed;
    Vector2 target;
    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            isMoving = !((Vector2)transform.position == target);
        }
    }

    public enum MovementType { Instant, Speed, Duration }
    public void Move(MovementType movementType, float timeFactor, Vector2 target)
    {
        switch (movementType)
        {
            case MovementType.Instant:
                transform.position = target;
                break;
            case MovementType.Speed:
                isMoving = true;
                speed = timeFactor;
                this.target = target;
                break;
            case MovementType.Duration:
                isMoving = true;
                speed = ((Vector3)target - transform.position).magnitude / timeFactor;
                this.target = target;
                break;
        }
    }

    public bool IsMoving { get => isMoving; }
}
