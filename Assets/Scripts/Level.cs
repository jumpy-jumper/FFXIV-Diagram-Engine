using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    public List<Actor> actors = new List<Actor>();

    public static Level Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public bool IsNameConflict(string str)
    {
        foreach (Actor actor in actors)
        {
            if (str == actor.name)
            {
                return true;
            }
        }

        return false;
    }

    void Start()
    {
        // Testing Commands
        SpawnCommand spawn = new SpawnCommand();
        spawn.name = "T1";
        spawn.spritePath = "Sprites/Jobs/GNB";
        spawn.Execute();
    }
}
