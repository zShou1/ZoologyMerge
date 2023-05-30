using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpawnUI : MonoBehaviour
{
    public Sprite spriteAnimal;

    public Image countdownImage;

    public Image animalImage;

    /*public float timeCountdown;*/
    public bool isCountdownFinished;
    private Animal currentAnimalTap;
    private GameObject currentAnimalTapGameObject;
    private Animal maxLevelAnimal;
    public bool isRollCountdownFinished;

    private void Start()
    {
        countdownImage.fillAmount = 0;
        isCountdownFinished = true;
        isRollCountdownFinished = true;
        UIManager.Instance.availableSpawnPoints =
            UIManager.Instance.spawnPoints.FindAll(spawnpoint =>
                spawnpoint.currentAnimal == null && spawnpoint.currentEnemy == null);
    }

    public void SpawnAnimal()
    {
        if (!isRollCountdownFinished)
        {
            return;
        }

        if (!isCountdownFinished)
        {
            return;
        }

        if (!CanSpawn())
        {
            return;
        }
        RandomAndSpawn();
    }

    private bool CanSpawn()
    {
        foreach (var spawnPoint in UIManager.Instance.spawnPoints)
        {
            if (spawnPoint.currentAnimal == null && spawnPoint.currentEnemy == null)
            {
                return true;
            }
        }

        return false;
    }

    private void RandomAndSpawn()
    {
        UIManager.Instance.availableSpawnPoints =
            UIManager.Instance.spawnPoints.FindAll(spawnpoint =>
                spawnpoint.currentAnimal == null && spawnpoint.currentEnemy == null);
        isCountdownFinished = false;
        /*Debug.Log(CanSpawn());*/
        // Nếu không còn SpawnPoint có thể sử dụng, không spawn động vật
/*        if (UIManager.Instance.availableSpawnPoints.Count == 0)
        {
            return;
        }*/

        // Chọn ngẫu nhiên một SpawnPoint từ danh sách availableSpawnPoints
        int randomIndex = Random.Range(0, UIManager.Instance.availableSpawnPoints.Count);
        SpawnPoint spawnPoint = UIManager.Instance.availableSpawnPoints[randomIndex];

        currentAnimalTap = UIManager.Instance.listAnimals.Find(animal => animal.animalSprite.name == spriteAnimal.name);
        currentAnimalTapGameObject =
            UIManager.Instance.animalPrefabs.Find(animalprefab =>
                animalprefab.name == currentAnimalTap.gameObject.name);

        // Spawn động vật tại SpawnPoint đã chọn

        spawnPoint.animalSpawnPoint =
            Instantiate(currentAnimalTapGameObject, spawnPoint.transform.position, Quaternion.identity);

        /*countdownImage.fillAmount = 1f;*/
        StartCoroutine(CountDown());

        /*spawnPoint.currentAnimal = animal.GetComponent<Animal>();*/

        // Loại bỏ SpawnPoint đã sử dụng khỏi danh sách availableSpawnPoints
        UIManager.Instance.availableSpawnPoints.RemoveAt(randomIndex);
    }


    public IEnumerator CountDown()
    {
        float countdown = currentAnimalTap.timeCountdown;
        /*Debug.Log("current Animal Countdown: "+ currentAnimalTap.timeCountdown);*/
        while (countdown > 0)
        {
            countdown -= Time.deltaTime;

            countdownImage.fillAmount = countdown / currentAnimalTap.timeCountdown;
            /*Debug.Log(countdown/currentAnimalTap.timeCountdown);*/
            yield return null;
        }

        yield return new WaitUntil(() => (countdown <= 0));
        isCountdownFinished = true;
    }

    public IEnumerator RollCountdown(float rollCountndownTime)
    {
        float countdown = rollCountndownTime;
        while (countdown > 0)
        {
            countdown -= Time.deltaTime;
            countdownImage.fillAmount = countdown / rollCountndownTime;
            yield return null;
        }

        yield return new WaitUntil(() => (countdown <= 0));
        isRollCountdownFinished = true;
    }


}
