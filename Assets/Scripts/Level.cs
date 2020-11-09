using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // Actor tracking
    public List<Actor> actors = new List<Actor>();
    public Dictionary<string, List<Actor>> groups = new Dictionary<string, List<Actor>>();
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
    public Stack<IExecutable> history = new Stack<IExecutable>();

    // Testing
    public Queue<ITestable> tests = new Queue<ITestable>();
    ITestable curTest;

    void Start()
    {
        inputWait = false;
        secondsWait = 0;

        tests.Enqueue(new ScratchTest());
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
            history.Push(command);
        }

        // If testing done, check and start a new test
        if (commands.Count == 0 && tests.Count > 0)
        {
            if (curTest != null)
            {
                curTest.Check(this);
            }
            curTest = tests.Dequeue();
            curTest.Build(this);
        }
    }
}
