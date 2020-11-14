using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // Actor tracking
    public List<Actor> actors = new List<Actor>();
    public Dictionary<string, List<Actor>> groups = new Dictionary<string, List<Actor>>();
    public List<Actor> GetActors(string name)
    {
        Actor actor = actors.Find(a => a.name == name);
        if (actor)
        {
            return new List<Actor>() { actor };
        }

        if (groups.ContainsKey(name))
        {
            return groups[name];
        }

        Debug.LogError("Get Actors: Actor with name \"" + name + "\" or in group \"" + name + "\" not found");
        return null;
    }
    public List<Actor> GetActors(List<string> names)
    {
        List<Actor> ret = new List<Actor>();

        foreach (string name in names)
        {
            Actor actor = actors.Find(a => a.name == name);
            if (actor)
            {
                ret.Add(actor);
            }
            else
            {
                Debug.LogError("Get Actors: Actor with name \"" + name + "\" not found");
                return null;
            }
        }

        return ret;
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

        foreach (string key in groups.Keys)
        {
            if (str == key)
            {
                return true;
            }
        }

        return false;
    }

    // Waiting logic
    public bool Waiting { get => inputWait || secondsWait > 0f; }
    public bool inputWait;
    public float secondsWait;

    // Commands
    public Queue<IExecutable> commands = new Queue<IExecutable>();

    void Start()
    {
        inputWait = false;
        secondsWait = 0;

        BuildTests();
    }

    void Update()
    {
        // Advance waiting logic
        secondsWait -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            inputWait = false;
        }

        // Process commands until wait is issued or there are no more commands
        while (!Waiting && commands.Count > 0)
        {
            IExecutable command = commands.Dequeue();
            command.Execute(this);
        }
    }

    void BuildTests()
    {
        commands.Enqueue(new SpawnCommand("T1", "Sprites/Jobs/GNB"));
        commands.Enqueue(new SpawnCommand("T2", "Sprites/Jobs/PLD"));
        commands.Enqueue(new WaitForInputCommand());
        commands.Enqueue(new MoveCommand("T1", Moveable.MovementType.Duration, 0.3f, new Vector2(-5, -1)));
        commands.Enqueue(new MoveCommand("T2", Moveable.MovementType.Duration, 0.3f, new Vector2(2, 1)));
        commands.Enqueue(new GroupCommand("tanks", new List<string>() { "T1", "T2" }));
        commands.Enqueue(new WaitForInputCommand());
        commands.Enqueue(new MoveCommand("tanks", Moveable.MovementType.Duration, 0.3f, Vector2.zero));
    }

}
