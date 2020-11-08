using UnityEngine;

/*
 * Instantiates a new actor and adds it to the level's actor list.
 */
public class SpawnCommand : IExecutable
{
    public string name;
    public string spritePath;

    public SpawnCommand()
    {
        name = "";
        spritePath = "";
    }

    public bool Execute(Level level)
    {
        // Check the name is valid.
        if (name == "")
        {
            Debug.LogError("Spawn Command: No name specified");
            return false;
        }
        if (level.IsNameConflict(name))
        {
            Debug.LogError("Spawn Command: Name conflict: \"" + name + "\"");
            return false;
        }

        // Check the sprite path is valid.
        Sprite spr = Resources.Load<Sprite>(spritePath);
        if (spritePath == "")
        {
            Debug.LogError("Spawn Command: No sprite path specified");
            return false;
        }
        if (!spr)
        {
            Debug.LogError("Spawn Command: Sprite not found at path \"" + spritePath + "\"");
            return false;
        }

        // Spawn the new actor.
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Actor"), level.transform);
        obj.name = name;
        obj.GetComponent<SpriteRenderer>().sprite = spr;
        level.actors.Add(obj.GetComponent<Actor>());

        return true;
    }
}
