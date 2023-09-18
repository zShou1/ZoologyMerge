using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PreviousLevelData
{
    public int LaneID;
    public int SpawnPointID;
    public int AnimalLevel;

    public PreviousLevelData(int LaneID, int SpawnPointID, int AnimalLevel)
    {
        this.LaneID = LaneID;
        this.SpawnPointID = SpawnPointID;
        this.AnimalLevel = AnimalLevel;
    }


    public virtual string ToString()
    {
        return $"Data : LaneID {LaneID}, SpawnPointID {SpawnPointID}, Animal Level {AnimalLevel} ";
    }
}
