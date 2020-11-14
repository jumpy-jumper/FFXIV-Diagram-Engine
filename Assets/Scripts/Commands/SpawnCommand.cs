using UnityEngine;

/*
 * Instantiates a new actor and adds it to the level's actor list.
 */
public class SpawnCommand : IExecutable
{
    readonly string name;
    readonly string spritePath;

    public SpawnCommand(string name, string spritePath)
    {
        this.name = name;
        this.spritePath = spritePath;
    }

    public bool Execute(Level level)
    {
        Sprite spr = Resources.Load<Sprite>(spritePath);
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Actor"), level.transform);
        obj.name = name;
        obj.GetComponent<SpriteRenderer>().sprite = spr;
        level.actors.Add(obj.GetComponent<Actor>());
        return true;
    }
}
