using UnityEngine;

/*
 * An actor adds itself to and removes itself from the level's actor list.
 */
public class Actor : MonoBehaviour
{
    void Awake()
    {
        Level.Instance.actors.Add(this);
    }

    void OnDestroy()
    {
        Level.Instance.actors.Remove(this);
    }
}
