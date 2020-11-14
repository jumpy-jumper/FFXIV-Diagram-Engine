using UnityEngine;

public class PositionController : MonoBehaviour
{
    public GameObject parent = null;
    public Vector2 offset;

    bool isMoving;
    float speed;
    Vector2 target;

    void Update()
    {
        if (parent)
        {
            transform.position = parent.transform.position + (Vector3)offset;
        }

        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            isMoving = !((Vector2)transform.position == target);
        }
    }

    public enum MovementType { Instant, Speed, Duration }
    public void Move(MovementType movementType, float timeFactor, Vector2 target)
    {
        if (parent)
        {
            return;
        }

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

    public void Lock(GameObject obj, Vector2 offset)
    {
        parent = obj;
        this.offset = offset;
    }

    public void Unlock()
    {
        parent = null;
    }

    public bool IsMoving { get => isMoving; }
}
