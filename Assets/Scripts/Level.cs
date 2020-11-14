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
        SpawnCommand s1 = new SpawnCommand();
        s1.name = "T1";
        s1.spritePath = "Sprites/Jobs/GNB";
        commands.Enqueue(s1);

        SpawnCommand s2 = new SpawnCommand();
        s2.name = "T2";
        s2.spritePath = "Sprites/Jobs/PLD";
        commands.Enqueue(s2);

        commands.Enqueue(new WaitForInputCommand());

        MoveCommand m1 = new MoveCommand();
        m1.label = "T1";
        m1.movementType = Moveable.MovementType.Duration;
        m1.target = new Vector2(-5, -1);
        m1.timeFactor = 0.3f;
        commands.Enqueue(m1);

        MoveCommand m2 = new MoveCommand();
        m2.label = "T2";
        m2.movementType = Moveable.MovementType.Duration;
        m2.target = new Vector2(2, 1);
        m2.timeFactor = 0.3f;
        commands.Enqueue(m2);

        GroupCommand g1 = new GroupCommand();
        g1.label = "tanks";
        g1.names = new List<string>() { "T1", "T2" };
        commands.Enqueue(g1);

        commands.Enqueue(new WaitForInputCommand());

        MoveCommand m3 = new MoveCommand();
        m3.label = "tanks";
        m3.movementType = Moveable.MovementType.Duration;
        m3.target = Vector2.zero;
        m3.timeFactor = 0.3f;
        commands.Enqueue(m3);
    }

}
