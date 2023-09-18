using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private List<LevelTable> _listLevel;
    [SerializeField] private LevelTable levelTable;
    public List<Lane> laneList;
    [SerializeField]
    private float timeDelayEnemy= 2f;
    Vector3 spawnEnemyOnLanePoint;
    private int currentEnemyDestroy;
    [SerializeField] private float timeDelayWave;

    [SerializeField] private GameObject EnemyPrefab;

    [SerializeField] private GameObject nextLevelPanel;

    private bool isWin = false;
/*    private List<Way> randomWayList;*/

    private void Awake()
    {
        nextLevelPanel.SetActive(false);
        
    }

    private void Start()
    {
        Time.timeScale = 1f;
        isWin = false;
        levelTable = _listLevel[GameManager.Instance.CurrentLevel-1];
        for (int i = 0; i < levelTable.waveList.Count; i++)
        {
            levelTable.waveList[i].TotalEnemy = 0;
            levelTable.waveList[i].randomWayList= new List<Way>();
            foreach (var way in levelTable.waveList[i].wayList)
            {
                if (way.enemyLevel>0)
                {
                    levelTable.waveList[i].TotalEnemy++;
                    levelTable.waveList[i].randomWayList.Add(way);
                    levelTable.waveList[i].randomWayList = levelTable.waveList[i].randomWayList
                        .OrderBy(x => Random.Range(0f, 1f)).ToList();
                }
            }
        }
        StartCoroutine(CreateLevel());
    }

    public IEnumerator CreateLevel()
    {
        for (int i = 0; i < laneList.Count; i++)
        {
            laneList[i].LaneType = levelTable.LaneTypeList[i];
        }
        for (int i = 0; i < levelTable.waveList.Count; i++)
        {
            currentEnemyDestroy = 0;

            LevelTable.Wave wave = levelTable.waveList[i];
            
            for (int j = 0; j < wave.randomWayList.Count; j++)
            {
                wave.randomWayList[j].enemyLevel = wave.waveNum;
                StartCoroutine(SpawnEnemyWay(wave.randomWayList[j]));
                yield return new WaitForSeconds(timeDelayEnemy);
            }

            yield return new WaitUntil(() => (currentEnemyDestroy == wave.TotalEnemy));
            foreach (var lane in laneList)
            {
                lane.EnemyOnLaneQueue.Clear();
            }
            yield return new WaitForSeconds(timeDelayWave);
            //Win khi hết quái ở Wave cuối
            if (i == levelTable.waveList.Count-1)
            {
             yield return new WaitForSeconds(1.5f);
             Time.timeScale = 0;
             nextLevelPanel.SetActive(true);
            }
        }
    }

    IEnumerator SpawnEnemyWay(Way way)
    {
        if(way.enemyLevel<=0) yield break;
        bool isNoLane = false;
        if (way.enemyLevel>0)
        {
            Lane lane = null;
            switch (way.LaneNum)
            {
                case LaneNum.Lane1:
                    lane = laneList[0];
                    spawnEnemyOnLanePoint = laneList[0].gameObject.transform.position +
                                            new Vector3(0, 0, laneList[0].laneHeight / 2)/* + new Vector3(0, 0, 0.4f)*/;
                    break;
                case LaneNum.Lane2:
                    lane = laneList[1];
                    spawnEnemyOnLanePoint = laneList[1].gameObject.transform.position +
                                            new Vector3(0, 0, laneList[1].laneHeight / 2) /*+ new Vector3(0, 0, 0.4f)*/;
                    break;
                case LaneNum.Lane3:
                    lane = laneList[2];
                    spawnEnemyOnLanePoint = laneList[2].gameObject.transform.position +
                                            new Vector3(0, 0, laneList[2].laneHeight / 2) /*+ new Vector3(0, 0, 0.4f)*/;
                    break;
                case LaneNum.Lane4:
                    lane = laneList[3];
                    spawnEnemyOnLanePoint = laneList[3].gameObject.transform.position +
                                            new Vector3(0, 0, laneList[3].laneHeight / 2) /*+ new Vector3(0, 0, 0.4f)*/;
                    break;
                case LaneNum.Lane5:
                    lane = laneList[4];
                    spawnEnemyOnLanePoint = laneList[4].gameObject.transform.position +
                                            new Vector3(0, 0, laneList[4].laneHeight / 2) /*+ new Vector3(0, 0, 0.4f)*/;
                    break;
                default:
                    isNoLane = true;
                    break;
            }
            
            if (isNoLane) yield break;
            var insEnemy = Instantiate(EnemyPrefab, spawnEnemyOnLanePoint, Quaternion.identity);
            Enemy enemy = insEnemy.GetComponent<Enemy>();
            enemy.InitEnemyLevel(way.enemyLevel);
            enemy.OnEnemyDestroy+= OnEnemyDestroyInWave;
            switch (lane.LaneType)
            {
                case LaneType.Slow:
                    enemy.Init(0.5f, 1.1f);
                    break;
                case LaneType.Normal:
                    enemy.Init(1, 1);
                    break;
                case LaneType.Fast:
                    enemy.Init(2f, 0.9f);
                    break;
            }
            
            // Add enemy to the lane's queue
            if (lane.EnemyOnLaneQueue == null)
            {
                lane.EnemyOnLaneQueue = new Queue<Enemy>();
            }
            lane.EnemyOnLaneQueue.Enqueue(enemy);
        }

        yield return new WaitForSeconds(timeDelayEnemy);
    }

    private void OnEnemyDestroyInWave()
    {
        currentEnemyDestroy++;
    }
}