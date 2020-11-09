using System.Collections.Generic;
using UnityEngine;

public class ScratchTest : ITestable
{
    public void Build(Level level)
    {
        SpawnCommand s1 = new SpawnCommand();
        s1.name = "T1";
        s1.spritePath = "Sprites/Jobs/GNB";
        level.commands.Enqueue(s1);

        SpawnCommand s2 = new SpawnCommand();
        s2.name = "T2";
        s2.spritePath = "Sprites/Jobs/PLD";
        level.commands.Enqueue(s2);
    }

    public bool Check(Level level)
    {
        bool passed = true;
        return passed;
    }
}
