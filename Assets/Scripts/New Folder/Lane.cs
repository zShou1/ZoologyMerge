using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Lane : MonoBehaviour
{
    public int laneID;
    
    private BoxCollider _boxCollider;

    public float laneHeight;

    public Animal currentAnimalOnLane;

    public GameObject animalOnLane;

    public Queue<Enemy> EnemyOnLaneQueue = new Queue<Enemy>();

    private float deltaDistanceAnimalVsEnemy = 2f;

    private GameObject laneLight;

    [HideInInspector] public LaneType LaneType;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        laneHeight = _boxCollider.size.y;

        laneLight = transform.GetChild(0).gameObject;
        laneLight.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(CheckStateOnLane());
    }

    public void OnActiveLaneLight()
    {
        laneLight.SetActive(true);
    }

    public void DeActiveLaneLight()
    {
        laneLight.SetActive(false);
    }

    public Vector3 AnimalOnLanePosition()
    {
        return transform.position -
               new Vector3(0, 0, laneHeight / 2) + new Vector3(0, 0, 0.2f);
    }

    IEnumerator CheckStateOnLane()
    {
        if (EnemyOnLaneQueue.Count > 0 && currentAnimalOnLane)
        {
            for (int i = 0; i < EnemyOnLaneQueue.Count; i++)
            {
                /*currentAnimalOnLane.isInitMoveToPlayer = false;*/
                /*if (!currentAnimalOnLane.isAttacking)
                {
                    currentAnimalOnLane.isAttacked = false;
                }*/
                Enemy firstEnemyInQueue = EnemyOnLaneQueue.Peek();
                firstEnemyInQueue.OnDequeue += OnDequeueOnLane;
                if (currentAnimalOnLane.animalLevel > firstEnemyInQueue.EnemyLevel)
                {
                    /*currentAnimalOnLane.InitMoveToEnemy(firstEnemyInQueue.transform, true);*/
                    /*Chuyen sang dung State*/
                    /*StartCoroutine(currentAnimalOnLane.MoveToEnemy(firstEnemyInQueue.transform));*/
                    currentAnimalOnLane.currentEnemyAttackTransform = firstEnemyInQueue.transform;
                    if (currentAnimalOnLane.currentState == AnimalState.Freeze)
                    {
                        currentAnimalOnLane.SetState(AnimalState.MoveToEnemy);
                    }
                    if (Vector3.Magnitude(firstEnemyInQueue.transform.position -
                                          currentAnimalOnLane.transform.position) <= currentAnimalOnLane.deltaDistance)
                    {
                        /*currentAnimalOnLane.Attack();*/
                        /*currentAnimalOnLane.currentState = AnimalState.Attack;*/
                        if (currentAnimalOnLane.currentState ==AnimalState.MoveToEnemy)
                        {
                            currentAnimalOnLane.SetState(AnimalState.Attack);
                        }
                        if (/*currentAnimalOnLane.isInitMoveToPlayer && */currentAnimalOnLane.isAttacked)
                        {
                            firstEnemyInQueue.OnDeActive();
                            Destroy(firstEnemyInQueue.transform.gameObject);
                            EnemyOnLaneQueue.Dequeue();
                        }
                    }
                    
                }
                else if (currentAnimalOnLane.animalLevel == firstEnemyInQueue.EnemyLevel)
                {
                    if (Vector3.Magnitude(firstEnemyInQueue.transform.position -
                                          currentAnimalOnLane.transform.position) <= currentAnimalOnLane.deltaDistance)
                    {
                        /*currentAnimalOnLane.Attack();*/
                        /*currentAnimalOnLane.currentState = AnimalState.Attack;*/
                        if (currentAnimalOnLane.currentState != AnimalState.Attack)
                        {
                            currentAnimalOnLane.SetState(AnimalState.Attack);
                        }
                        if (currentAnimalOnLane.isAttacked)
                        {
                            firstEnemyInQueue.OnDeActive();
                            Destroy(firstEnemyInQueue.transform.gameObject);
                            EnemyOnLaneQueue.Dequeue();
                            Destroy(currentAnimalOnLane.gameObject, 0.4f);
                            currentAnimalOnLane = null;
                            animalOnLane = null;
                        }
                    }

                    /*StartCoroutine(currentAnimalOnLane.AttackEnemySameLevel(firstEnemyInQueue.transform));*/
                    
                }
                else if (currentAnimalOnLane.animalLevel < firstEnemyInQueue.EnemyLevel)
                {
                    if (Vector3.Magnitude(firstEnemyInQueue.transform.position -
                                          currentAnimalOnLane.transform.position) <= currentAnimalOnLane.deltaDistance)
                    {
                        /*currentAnimalOnLane.Attack();*/
                        if (currentAnimalOnLane.currentState != AnimalState.Attack)
                        {
                            currentAnimalOnLane.SetState(AnimalState.Attack);
                        }
                        
                        if (currentAnimalOnLane.isAttacked)
                        {
                            /*Destroy(firstEnemyInQueue.transform.gameObject);
                            EnemyOnLaneQueue.Dequeue();*/
                            Destroy(currentAnimalOnLane.gameObject, 0.4f);
                            currentAnimalOnLane = null;
                            animalOnLane = null;
                        }
                    }

                    /*StartCoroutine(currentAnimalOnLane.AttackEnemySameLevel(firstEnemyInQueue.transform));*/
                    
                }
                yield return new WaitUntil(() => firstEnemyInQueue.isDestroyOrThrough);
                if (currentAnimalOnLane)
                {
                    currentAnimalOnLane.isAttacked = false;
                }
            }
        }

        if (EnemyOnLaneQueue.Count == 0 && currentAnimalOnLane)
        {
            if (currentAnimalOnLane.isInitMoveToPlayer && currentAnimalOnLane.isAttacked)
            {
                /*currentAnimalOnLane.MoveToAnimalOnLanePosition();*/
                currentAnimalOnLane.currentState = AnimalState.MoveToLanePosition;
            }
        }
    }
    private void FixedUpdate()
    {
        StartCoroutine(CheckStateOnLane());

    }

    private void OnDequeueOnLane()
    {
        if (EnemyOnLaneQueue.Count > 0)
        {
            EnemyOnLaneQueue.Dequeue();
        }
    }
}