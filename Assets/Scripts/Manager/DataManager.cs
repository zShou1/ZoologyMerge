using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public List<PreviousLevelData> previousLevelDataList;
    private string path = "";
    private string persistentPath = "";
    private List<SpawnPoint> SpawnPoints;
    private List<Lane> Lanes;

    private void Start()
    {
        SpawnPoints = UIManager.Instance.spawnPoints;
        Lanes = LevelManager.Instance.laneList;
        /*CreateData();*/
        SetPaths();
        if (GameManager.Instance.CurrentLevel > 1)
        {
            LoadData();
            CreateAnimalSaved();
        }
    }

    private void Update()
    {
/*        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData();
        }*/
    }

    private void CreateAnimalSaved()
    {
        foreach (PreviousLevelData previousLevelData in previousLevelDataList)
        {
            Animal AnimalSpawn =
                UIManager.Instance.listAnimals.Find(x => x.animalLevel == previousLevelData.AnimalLevel);
            GameObject AnimalSpawnObject =
                UIManager.Instance.animalPrefabs.Find(x => x.name == AnimalSpawn.gameObject.name);
            if (previousLevelData.LaneID == 0)
            {
                SpawnPoint spawnPoint= SpawnPoints.Find(sp => sp.spawnPointID == previousLevelData.SpawnPointID);
                spawnPoint.animalSpawnPoint =
                    Instantiate(AnimalSpawnObject, spawnPoint.transform.position, Quaternion.identity);
            }

            if (previousLevelData.SpawnPointID == 0)
            {
                Lane lane = Lanes.Find(l => l.laneID == previousLevelData.LaneID);
                Instantiate(AnimalSpawnObject, lane.AnimalOnLanePosition(), Quaternion.identity);
            }
        }
    }
    private void CreateData()
    {
        
        foreach (SpawnPoint spawnPoint in SpawnPoints)
        {
            //Duyệt qua List SpawnPoint, ô nào chứa Animal thì thêm vào previousLevelData để Save
            if (spawnPoint.currentAnimal != null)
            {
                /*Debug.Log(spawnPoint.name);*/
                PreviousLevelData previousLevelData= new PreviousLevelData(0, spawnPoint.spawnPointID, spawnPoint.currentAnimal.animalLevel);
                /*Debug.Log(previousLevelData.ToString());*/
                previousLevelDataList.Add(previousLevelData);
            }
        }
        
        foreach (Lane lane in Lanes)
        {
            //Duyệt qua List SpawnPoint, ô nào chứa Animal thì thêm vào previousLevelData để Save
            if (lane.currentAnimalOnLane != null)
            {
                Debug.Log(lane.name);
                PreviousLevelData previousLevelData= new PreviousLevelData(lane.laneID, 0, lane.currentAnimalOnLane.animalLevel);
                previousLevelDataList.Add(previousLevelData);
            }
        }
    }

    private void SetPaths()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";

        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
    }

    public void SaveData()
    {
        //Trước khi tạo Data Save mới thì xóa dữ liệu cũ đi
        previousLevelDataList.Clear();
        CreateData();
        string savePath = persistentPath;
        
        Debug.Log("Save Data at: "+ savePath);
        SaveData saveData= new SaveData
        {
            //Tạo đối tượng SaveData để bao quanh danh sách previousLevelDataList
            PreviousLevelDataList = previousLevelDataList
        };

        using StreamWriter writer= new StreamWriter(savePath);
        
        string json = JsonUtility.ToJson(saveData);
        Debug.Log(json);
            
        writer.WriteLine(json);
    }

    public void LoadData()
    {
        using (StreamReader reader = new StreamReader(persistentPath))
        {
            string json = reader.ReadToEnd();

            // Chuyển đổi chuỗi JSON thành đối tượng SaveData
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            previousLevelDataList = saveData.PreviousLevelDataList;

            /*// Truy cập danh sách previousLevelDataList từ đối tượng SaveData
            foreach (PreviousLevelData data in saveData.PreviousLevelDataList)
            {
                Debug.Log(data.ToString());
            }*/
        }
    }
}