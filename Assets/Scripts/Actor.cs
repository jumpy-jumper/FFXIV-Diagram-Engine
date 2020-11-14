using UnityEngine;

public class Actor : MonoBehaviour
{
    PositionController moveableComponent;
    public bool IsAnimating { get => moveableComponent.IsMoving; }

    void Awake()
    {
        moveableComponent = GetComponent<PositionController>();
    }
}
