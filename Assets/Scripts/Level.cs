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

    // Command logic
    public Queue<IExecutable> commands = new Queue<IExecutable>();
    public Stack<IExecutable> history = new Stack<IExecutable>();

    void Start()
    {
        inputWait = false;
        secondsWait = 0;
    }

    void Update()
    {
        // Process waiting logic
        secondsWait = Mathf.Max(secondsWait - Time.deltaTime, 0f);
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            inputWait = false;
        }

        // Process commands until wait is issued or no more commands
        while (!Waiting && commands.Count > 0)
        {
            IExecutable command = commands.Dequeue();
            command.Execute(this);
            history.Push(command);
        }
    }
}
