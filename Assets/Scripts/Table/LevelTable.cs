using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Create LevelTable", fileName = "LevelTable")]
public class LevelTable : ScriptableObject
{

    [Serializable]
    public class Wave
    {
        [SerializeField] public List<Way> wayList;

        public List<Way> randomWayList;

        public int TotalEnemy=0;
    }

    [SerializeField] public List<Wave> waveList;


}