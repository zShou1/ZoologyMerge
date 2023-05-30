using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Animal testAnimal;
    [SerializeField] private GameObject testAnimalPrefab;
    [SerializeField] private List<Image> animalImages;

    [SerializeField] private List<TextMeshProUGUI> levelAnimalSpawnTexts;
    public List<SpawnPoint> spawnPoints;
    public List<SpawnPoint> availableSpawnPoints = new List<SpawnPoint>();
    public List<Animal> listAnimals;

    [SerializeField] private List<SpawnUI> SpawnUIs;
    public List<Animal> availableAnimals;
    private List<int[]> weightRollEachAnimals = new List<int[]>();

    public List<GameObject> animalPrefabs;

    public SpawnUI spawnUIMaxLevelAnimal;

    private float rollCountdownTime = 1f;
    
    
    void Awake()
    {
        CreateListWeight();
        availableAnimals.Clear();
        Roll();
        SetMaxLevelAnimalPanel();
    }
    private void Start()
    {
        
        /*// Tìm tất cả các SpawnPoint có thể sử dụng và thêm vào danh sách availableSpawnPoints
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            if (spawnPoint.CanSpawn())
            {
                availableSpawnPoints.Add(spawnPoint);
            }
        }*/
        
        PopListWeightToListAnimals();
    }

    private void CreateListWeight()
    {
        int[] Rooster = {20, 18, 16, 15, 14, 13, 12, 10, 8, 6, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        int[] Koala = {0, 2, 4, 5, 6, 6, 6, 7, 5, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        int[] Dog = {0, 0, 0, 0, 1, 1, 2, 3, 4, 5, 5, 3, 2, 0, 0, 0, 0, 0, 0, 0};
        int[] Peacock = {0, 0, 0, 0, 0, 0, 0, 1, 1, 3, 6, 5, 3, 2, 0, 0, 0, 0, 0, 0};
        int[] Flamingo = {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 6, 6, 3, 2, 0, 0, 0, 0, 0};
        int[] Pig = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 6, 6, 3, 2, 0, 0, 0, 0};
        int[] Tortoise = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 6, 6, 3, 2, 0, 0, 0};
        int[] Zebra = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 6, 6, 3, 2, 0, 0};
        int[] Tiger = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 6, 6, 3, 2, 0};
        int[] Panda = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 6, 6, 3, 2};
        int[] Gorilla = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 6, 6, 3};
        int[] Elephant = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 6, 6};
        int[] Snake = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 6};
        int[] Crocodile = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2};
        
        weightRollEachAnimals.Add(Rooster);
        weightRollEachAnimals.Add(Koala);
        weightRollEachAnimals.Add(Dog);
        weightRollEachAnimals.Add(Peacock);
        weightRollEachAnimals.Add(Flamingo);
        weightRollEachAnimals.Add(Pig);
        weightRollEachAnimals.Add(Tortoise);
        weightRollEachAnimals.Add(Zebra);
        weightRollEachAnimals.Add(Tiger);
        weightRollEachAnimals.Add(Panda);
        weightRollEachAnimals.Add(Gorilla);
        weightRollEachAnimals.Add(Elephant);
        weightRollEachAnimals.Add(Snake);
        weightRollEachAnimals.Add(Crocodile);
        /*foreach (var weightRollEachAnimal in weightRollEachAnimals)
        {
            Debug.Log(weightRollEachAnimal[GameManager.Instance.CurrentLevel-1]);
        }*/
    }

    private void PopListWeightToListAnimals()
    {
        //Pop weightRoll của từng con theo level vào list availableAnimals
        for (int i = 0; i < listAnimals.Count; i++)
        {
            listAnimals[i].weightRoll = weightRollEachAnimals[i][GameManager.Instance.CurrentLevel-1];
            /*Debug.Log(listAnimals[i].weightRoll);*/
        }
    }

    /*public void Roll()
    {
        int randomIndex = Random.Range(0, listAnimals.Count);
        Debug.Log(randomIndex);
        SpawnUIs[0].imageAnimal.sprite = listAnimals[randomIndex].animalSprite;
        SpawnUIs[1].imageAnimal.sprite = listAnimals[randomIndex].animalSprite;
        SpawnUIs[0].spriteAnimal = listAnimals[randomIndex].animalSprite;
        SpawnUIs[1].spriteAnimal = listAnimals[randomIndex].animalSprite;
        /*SpawnUI.Instance.timeCountdown = listAnimals[randomIndex].timeCountdown;#1#
        
        levelAnimalSpawnTexts[0].SetText("Level " + listAnimals[randomIndex].animalLevel);
        levelAnimalSpawnTexts[1].SetText("Level " + listAnimals[randomIndex].animalLevel);
    }
    */
    public void SetMaxLevelAnimalPanel()
    {
        Animal maxLevelAnimal = listAnimals.Find(animal => animal.animalLevel == GameManager.Instance.LevelMaxAnimal);
        SpawnUIs[2].isRollCountdownFinished = false;
        StartCoroutine(SpawnUIs[2].RollCountdown(rollCountdownTime));
        SpawnUIs[2].spriteAnimal = maxLevelAnimal.animalSprite;
        SpawnUIs[2].animalImage.sprite = SpawnUIs[2].spriteAnimal;
        levelAnimalSpawnTexts[2].SetText("Level " + maxLevelAnimal.animalLevel);
    }

    private void RollCountdown()
    {
        SpawnUIs[0].isRollCountdownFinished = false;
        SpawnUIs[1].isRollCountdownFinished = false;
        /*Debug.Log(SpawnUIs[0].isRollCountdownFinished);
        Debug.Log(SpawnUIs[1].isRollCountdownFinished);*/
        /*StopCoroutine(SpawnUIs[0].CountDown());
        StopCoroutine(SpawnUIs[1].CountDown());*/
        SpawnUIs[0].StopAllCoroutines();
        SpawnUIs[0].isCountdownFinished = true;
        SpawnUIs[1].StopAllCoroutines();
        SpawnUIs[1].isCountdownFinished = true;
        StartCoroutine(SpawnUIs[0].RollCountdown(rollCountdownTime));
        StartCoroutine(SpawnUIs[1].RollCountdown(rollCountdownTime));
    }

    public void RollPress()
    {
        if(GameManager.Instance.GoldRoll<100) return;
        GameManager.Instance.GoldRoll -= 100;
        Roll();
    }
    private void Roll()
    {
        //Lấy ra List các Animal ở Level hiện tại theo Current Level vì các Animal có animalLevel lớn hơn current Level có trọng số weightRoll=0
        availableAnimals = listAnimals.FindAll(animal => animal.animalLevel <= GameManager.Instance.CurrentLevel);
        //Sắp xếp lại list availableAnimals theo weightRoll từ lớn đến bé
        availableAnimals.Sort((animail1, animal2)=> -animail1.weightRoll.CompareTo(animal2.weightRoll));
        if (availableAnimals.Count == 0)
        {
            Debug.Log("There are no available animals for the current level");
            return;
        }
        
        int totalWeightRandom = 20;
        
        int randomValue1 = Random.Range(0, totalWeightRandom);
        int randomValue2 = Random.Range(0, totalWeightRandom);
        /*bool isFound1 = false;
        bool isFound2=false;*/
        RollCountdown();
        
        /*Debug.Log(randomValue1+"a");*/
        /*Debug.Log(randomValue2+"s");*/
        
        for (int i = 0; i < availableAnimals.Count; i++)
        {
            
            /*Debug.Log(availableAnimals[i].name+": "+availableAnimals[i].weightRoll);*/
            if (randomValue1<availableAnimals[i].weightRoll)
            {
                /*Debug.Log(availableAnimals[i]);*/
                SpawnUIs[0].spriteAnimal = availableAnimals[i].animalSprite;
                SpawnUIs[0].animalImage.sprite = SpawnUIs[0].spriteAnimal;
                levelAnimalSpawnTexts[0].SetText("Level " + availableAnimals[i].animalLevel);
                /*isFound1 = true;*/
                break;
            }

            randomValue1 -= availableAnimals[i].weightRoll;

        }
        
        for (int i = 0; i < availableAnimals.Count; i++)
        {
            if (randomValue2 < availableAnimals[i].weightRoll)
            {
                SpawnUIs[1].spriteAnimal = availableAnimals[i].animalSprite;
                SpawnUIs[1].animalImage.sprite = SpawnUIs[1].spriteAnimal;
                levelAnimalSpawnTexts[1].SetText("Level " + availableAnimals[i].animalLevel);
                /*isFound2 = true;*/
                break;
            }
            
            randomValue2 -= availableAnimals[i].weightRoll;
            
        }
    }
    
}