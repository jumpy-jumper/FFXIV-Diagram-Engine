using UnityEngine;

public class Actor : MonoBehaviour
{
    Moveable moveableComponent;
    public bool IsAnimating { get => moveableComponent.IsMoving; }

    void Awake()
    {
        moveableComponent = GetComponent<Moveable>();
    }
}
