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
    public LinkedList<IExecutable> commands = new LinkedList<IExecutable>();
    public LinkedListNode<IExecutable> curCommand;

    void Start()
    {
        inputWait = false;
        secondsWait = 0;
        curCommand = null;

        BuildTests();

        // The stage always ends with a wait for input command before restarting
        // the scene.
        commands.AddLast(new WaitForInputCommand());
        commands.AddLast(new RestartSceneCommand());
    }

    void Update()
    {
        // Advance waiting state
        secondsWait -= Time.deltaTime;

        // Process user input
        if (inputWait)
        {
            // Space key or mouse click advance the stage
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                inputWait = false;
            }

            // Z or right click undos until the last WaitForInputCommand
            if ((Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(1)) && curCommand != commands.First)
            {
                do
                {
                    curCommand.Previous.Value.Reverse(this);
                    curCommand = curCommand.Previous;
                } while (curCommand.Previous != null && curCommand.Previous.Value.GetType() != typeof(WaitForInputCommand));
            }

            // Left arrow undos a single command
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && curCommand != commands.First)
            {
                curCommand.Previous.Value.Reverse(this);
                curCommand = curCommand.Previous;
            }

            // Right arrow advances a single command;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                curCommand.Value.Execute(this);
                curCommand = curCommand.Next;
            }
        }

        // Process commands until wait is issued
        while (!Waiting)
        {
            curCommand.Value.Execute(this);
            curCommand = curCommand.Next;
        }
    }

    void BuildTests()
    {
        commands.AddLast(new SpawnCommand("T1", "Sprites/Jobs/GNB"));
        commands.AddLast(new SpawnCommand("T2", "Sprites/Jobs/PLD"));
        commands.AddLast(new WaitForInputCommand());
        commands.AddLast(new MoveCommand("T1", Moveable.MovementType.Duration, 0.3f, new Vector2(-5, -1)));
        commands.AddLast(new MoveCommand("T2", Moveable.MovementType.Duration, 0.3f, new Vector2(2, 1)));
        commands.AddLast(new GroupCommand("tanks", new List<string>() { "T1", "T2" }));
        commands.AddLast(new WaitForInputCommand());
        commands.AddLast(new MoveCommand("tanks", Moveable.MovementType.Duration, 0.3f, new Vector2(3, 0)));
        curCommand = commands.First;
    }

}
