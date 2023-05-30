using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int CurrentLevel
    {
        set
        {
            PlayerPrefs.SetInt("currentLevel", value);
        }
        get { return PlayerPrefs.GetInt("currentLevel", 1); }
        
    }
    public int GoldRoll
    {
        set
        {
            PlayerPrefs.SetInt("goldRoll", value);
        }
        get { return PlayerPrefs.GetInt("goldRoll", 99000); }
    }

    public int LevelMaxAnimal
    {
        set
        {
            PlayerPrefs.SetInt("maxLevelAnimal", value);
        }
        get { return PlayerPrefs.GetInt("maxLevelAnimal", 1); }
    }
    public static int Heart= 3;
    public int EnemySpawnIndex = 0;
}
