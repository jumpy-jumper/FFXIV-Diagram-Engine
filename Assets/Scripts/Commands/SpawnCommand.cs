using UnityEngine;

/*
 * Instantiates a new actor and adds it to the stage's actor list.
 */
public class SpawnCommand : IExecutable
{
    readonly string name;
    readonly string spritePath;

    public SpawnCommand(string name, string spritePath)
    {
        this.name = name;
        this.spritePath = spritePath;
        obj = null;
    }

    Actor obj;
    public bool Execute(Stage stage)
    {
        Sprite spr = Resources.Load<Sprite>(spritePath);
        obj = GameObject.Instantiate<Actor>(Resources.Load<Actor>("Actor"), stage.transform);
        obj.name = name;
        obj.GetComponent<SpriteRenderer>().sprite = spr;
        stage.actors.Add(obj.GetComponent<Actor>());
        return true;
    }

    public bool Reverse(Stage stage)
    {
        stage.actors.Remove(obj);
        GameObject.Destroy(obj.gameObject);
        return true;
    }
}
