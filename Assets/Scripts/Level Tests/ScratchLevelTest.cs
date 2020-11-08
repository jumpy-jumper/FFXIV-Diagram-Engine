using System.Collections.Generic;
using UnityEngine;

public class ScratchLevelTest : MonoBehaviour
{
    Level level;

    void Start()
    {
        level = GetComponent<Level>();

        SpawnCommand s1 = new SpawnCommand();
        s1.name = "T1";
        s1.spritePath = "Sprites/Jobs/GNB";
        level.commands.Enqueue(s1);

        level.commands.Enqueue(new WaitForInputCommand());

        SpawnCommand s2 = new SpawnCommand();
        s2.name = "T2";
        s2.spritePath = "Sprites/Jobs/PLD";
        level.commands.Enqueue(s2);

        level.commands.Enqueue(new WaitForInputCommand());

        GroupCommand g1 = new GroupCommand();
        g1.label = "tanks";
        g1.names = new List<string>() { "T1", "T2" };
        level.commands.Enqueue(g1);

        level.commands.Enqueue(new WaitForInputCommand());
    }
}
