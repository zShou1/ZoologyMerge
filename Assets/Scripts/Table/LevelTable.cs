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
        //Vì các EnemyLevel trong 1 Wave đều bằng nhau nên tạo WaveNum rồi set cho EnemyLevel
        [SerializeField] public int waveNum;
        
        [SerializeField] public List<Way> wayList;

        public List<Way> randomWayList;

        public int TotalEnemy=0;
    }

    // Sử dụng private field để giới hạn 5 LaneType
    [SerializeField]
    private List<LaneType> laneTypeList;

    // Getter cho danh sách LaneType
    public List<LaneType> LaneTypeList
    {
        get
        {
            if (laneTypeList == null)
            {
                laneTypeList = new List<LaneType>();
                for (int i = 0; i < 5; i++)
                {
                    // Đảm bảo danh sách có ít nhất 5 phần tử, nếu không set gì thì để default là Normal
                    laneTypeList.Add(LaneType.Normal); 
                }
            }
            return laneTypeList;
        }
    }
    
    [SerializeField] public List<Wave> waveList;

    
}