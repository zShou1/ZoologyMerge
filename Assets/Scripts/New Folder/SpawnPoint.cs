using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Animal currentAnimal;
    
    public GameObject animalSpawnPoint;

    public Enemy currentEnemy;

    private GameObject spawnPointLight;
    
    
    private void Awake()
    {
        spawnPointLight = transform.GetChild(0).gameObject;
        spawnPointLight.SetActive(false);
    }

    /*private void OnEnableSpawnPointLight()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.up, out hitInfo, 5f, LayerMask.GetMask("Animal"),
            QueryTriggerInteraction.Collide))
        {
            Drag drag = hitInfo.collider.GetComponent<Drag>();

            if (hitInfo.collider.transform != null)
            {
                if (drag.isActiveSpawnPointLight)
                {
                    spawnPointLight.SetActive(true);
                }
                else
                {
                    spawnPointLight.SetActive(false);
                }
            }
        }
        else
        {
            spawnPointLight.SetActive(false);
        }

    }*/

    public void OnActiveLight()
    {
        spawnPointLight.SetActive(true);
    }

    public void DeActiveLight()
    {
        spawnPointLight.SetActive(false);
    }
    private void Update()
    {
        /*OnEnableSpawnPointLight();*/
    }
}
