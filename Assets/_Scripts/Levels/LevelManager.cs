using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager _instance;
    public Level level1, level2, level3, level4;

    public List<Level> allLevels = new List<Level>();
    
    private void Awake()
    {
        if(_instance == null) _instance = this;
        else Destroy(this);

        allLevels.Add(level1.GetLevelInstance());
        allLevels.Add(level2.GetLevelInstance());
        allLevels.Add(level3.GetLevelInstance());
        allLevels.Add(level4.GetLevelInstance());
    }

    public Level GetLevel1()
    {
        return allLevels.ElementAt(0);
    }

    public Level GetNextLevel(Level actualLevel)
    {
        return allLevels.ElementAt(actualLevel.level+1);
    }
}
