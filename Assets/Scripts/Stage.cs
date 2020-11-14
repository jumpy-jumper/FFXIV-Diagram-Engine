using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            List<Actor> ret = new List<Actor>();
            foreach (Actor groupMember in groups[name])
            {
                if (groupMember && groupMember.gameObject.activeSelf)
                {
                    ret.Add(groupMember);
                }
            }
            return ret;
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
        }

        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(1)))
        {
            UndoSnippet();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            UndoSingle();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AdvanceSingle();
        }

        // Process commands until wait is issued
        while (!Waiting)
        {
            curCommand.Value.Execute(this);
            curCommand = curCommand.Next;
        }
    }

    // A "snippet" is a section of commands bound by WaitForInput commands
    public void UndoSnippet()
    {
        if (curCommand != commands.First)
        {
            do
            {
                curCommand.Previous.Value.Reverse(this);
                curCommand = curCommand.Previous;
            } while (curCommand.Previous != null && curCommand.Previous.Value.GetType() != typeof(WaitForInputCommand));
        }
    }

    public void UndoSingle()
    {
        if (curCommand != commands.First)
        {
            curCommand.Previous.Value.Reverse(this);
            curCommand = curCommand.Previous;
        }
    }

    public void AdvanceSingle()
    {
        curCommand.Value.Execute(this);
        curCommand = curCommand.Next;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void BuildTests()
    {
        commands.AddLast(new SpawnCommand("T1", "Sprites/Jobs/GNB"));
        commands.AddLast(new SpawnCommand("T2", "Sprites/Jobs/PLD"));
        commands.AddLast(new WaitForInputCommand());
        commands.AddLast(new MoveCommand("T1", PositionController.MovementType.Duration, 0.3f, new Vector2(-5, -1)));
        commands.AddLast(new MoveCommand("T2", PositionController.MovementType.Duration, 0.3f, new Vector2(2, 1)));
        commands.AddLast(new GroupCommand("tanks", new List<string>() { "T1", "T2" }));
        commands.AddLast(new WaitForInputCommand());
        commands.AddLast(new MoveCommand("tanks", PositionController.MovementType.Duration, 0.3f, new Vector2(3, 0)));
        commands.AddLast(new WaitForInputCommand());
        commands.AddLast(new DespawnCommand("T1"));
        commands.AddLast(new SpawnCommand("H1", "Sprites/Jobs/WHM"));
        commands.AddLast(new MoveCommand("tanks", PositionController.MovementType.Duration, 0.3f, new Vector2(-1, -1)));
        commands.AddLast(new WaitForInputCommand());
        commands.AddLast(new ColorCommand("T2", Color.red));
        commands.AddLast(new ColorCommand("H1", Color.blue));
        commands.AddLast(new WaitForInputCommand());
        commands.AddLast(new LockCommand("H1", "T2", new Vector2(2, 1)));
        commands.AddLast(new MoveCommand("T2", PositionController.MovementType.Duration, 0.3f, new Vector2(0, 3)));
        commands.AddLast(new WaitForInputCommand());
        commands.AddLast(new UnlockCommand("H1"));
        commands.AddLast(new MoveCommand("T2", PositionController.MovementType.Duration, 0.3f, new Vector2(0, -3)));
        commands.AddLast(new WaitForInputCommand());
        curCommand = commands.First;
    }
}
