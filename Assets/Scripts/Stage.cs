using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
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
    public Stack<IExecutable> commands = new Stack<IExecutable>();
    public Stack<IExecutable> history = new Stack<IExecutable>();

    void Start()
    {
        actors.Clear();
        groups.Clear();

        inputWait = false;
        secondsWait = 0;

        commands.Clear();
        commands.Push(new WaitForInputCommand());
        BuildTests();

        history.Clear();
    }

    void Update()
    {
        // Advance waiting logic
        secondsWait -= Time.deltaTime;
        if (inputWait)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                inputWait = false;

                // If no more commands left to execute, restart stage
                if (commands.Count == 0)
                {
                    Undo(UndoType.All);
                    Start();
                }
            }
            else if (history.Count > 0 && Input.GetKeyDown(KeyCode.Z))
            {
                Undo(UndoType.UntilWaitForInputCommand);
            }
        }

        // Process commands until wait is issued or there are no more commands
        while (!Waiting && commands.Count > 0)
        {
            IExecutable command = commands.Pop();
            command.Execute(this);
            history.Push(command);
        }
    }

    enum UndoType { Single, UntilWaitForInputCommand, All }
    void Undo(UndoType undoType)
    {
        bool ShouldStop()
        {
            switch (undoType)
            {
                case UndoType.Single:
                    return true;
                case UndoType.UntilWaitForInputCommand:
                    return history.Peek().GetType() == typeof(WaitForInputCommand);
                default:
                    return false;
            }
        }

        do
        {
            IExecutable last = history.Pop();
            last.Reverse(this);
            commands.Push(last);
        } while (!(history.Count == 0 || ShouldStop()));
    }

    void BuildTests()
    {
        commands.Push(new MoveCommand("tanks", Moveable.MovementType.Duration, 0.3f, new Vector2(3, 0)));
        commands.Push(new WaitForInputCommand());
        commands.Push(new GroupCommand("tanks", new List<string>() { "T1", "T2" }));
        commands.Push(new MoveCommand("T2", Moveable.MovementType.Duration, 0.3f, new Vector2(2, 1)));
        commands.Push(new MoveCommand("T1", Moveable.MovementType.Duration, 0.3f, new Vector2(-5, -1)));
        commands.Push(new WaitForInputCommand());
        commands.Push(new SpawnCommand("T2", "Sprites/Jobs/PLD"));
        commands.Push(new SpawnCommand("T1", "Sprites/Jobs/GNB"));
    }

}
