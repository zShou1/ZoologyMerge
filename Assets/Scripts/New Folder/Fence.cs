using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fence : MonoBehaviour
{
    private void OnCollisionEnter(Collision cl)
    {
        if (cl.collider.CompareTag("Enemy"))
        {
            Debug.Log("aaa");
            cl.collider.GetComponent<Enemy>().MoveToSpawnPoint();
        }
    }
}
