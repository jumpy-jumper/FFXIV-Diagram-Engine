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
        while (!Waiting && curCommand != null)
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
                LinkedListNode<IExecutable> previous = (curCommand == null ? commands.Last : curCommand.Previous);
                previous.Value.Reverse(this);
                curCommand = previous;
            } while (curCommand.Previous != null && curCommand.Previous.Value.GetType() != typeof(WaitForInputCommand));
            inputWait = true;
        }
    }

    public void UndoSingle()
    {
        if (curCommand != commands.First)
        {
            LinkedListNode<IExecutable> previous = (curCommand == null ? commands.Last : curCommand.Previous);
            previous.Value.Reverse(this);
            curCommand = previous;
            inputWait = true;
        }
    }

    public void AdvanceSingle()
    {
        if (curCommand != commands.Last)
        {
            curCommand.Value.Execute(this);
            curCommand = curCommand.Next;
        }
    }

    void BuildTests()
    {
        commands.AddLast(new WaitForInputCommand());
        commands.AddLast(new SpawnCommand("T1", "Sprites/Jobs/GNB"));
        commands.AddLast(new SpawnCommand("T2", "Sprites/Jobs/PLD"));
        commands.AddLast(new WaitForInputCommand()); 
        commands.AddLast(new MoveCommand("T1", PositionController.MovementType.Duration, 0.3f, new Vector2(-5, -1)));
        commands.AddLast(new MoveCommand("T2", PositionController.MovementType.Duration, 0.3f, new Vector2(2, 1)));
        curCommand = commands.First;
    }
}
