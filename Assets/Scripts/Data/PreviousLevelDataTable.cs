using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create PreviousLevelDataTable", fileName = "PreviousLevelDataTable")]
public class PreviousLevelDataTable : ScriptableObject
{
    [Serializable]
    public class PreviousLevelData
    {
        public int LaneID;
        public int SpawnPointID;
        public Animal Animal;
        public Enemy Enemy;
    }
    
    public PreviousLevelData[] PreviousLevelDataList;
 
    
}
