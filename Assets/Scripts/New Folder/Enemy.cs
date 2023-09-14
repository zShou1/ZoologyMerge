using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public int EnemyLevel;
    private float speed = 0.5f;
    private float scaleIndex;
    private TextMeshPro enemyLevelText;

    public bool isDestroyOrThrough = false;

    private Animator _animator;


    public void InitEnemyLevel(int level)
    {
        EnemyLevel = level;
    }
    public void Init( float _speedConst, float _scaleConst)
    {
        speed *= _speedConst;
        scaleIndex = _scaleConst;
        StartMove();
    }

    private Rigidbody rb;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        enemyLevelText = GetComponentInChildren<TextMeshPro>();
        _animator = transform.GetChild(1).GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetFloat("Velocity", Math.Abs(rb.velocity.z));
    }

    private void StartMove()
    {
        transform.localScale*=scaleIndex;
    }

    private void FixedUpdate()
    {
        rb.velocity = -transform.forward * speed* Time.fixedDeltaTime*10f;
    }

    private void Start()
    {
        enemyLevelText.SetText(EnemyLevel.ToString());
    }
    
    public void MoveToSpawnPoint()
    {
        OnDeActive();
        OnActiveDequeue();
        enemyLevelText.gameObject.SetActive(false);
        rb.velocity= Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y,-1f*transform.localScale.z) ;
        /*int randomIndex = Random.Range(0, UIManager.Instance.availableSpawnPoints.Count);*/
        SpawnPoint spawnPoint = UIManager.Instance.spawnPoints[GameManager.Instance.EnemySpawnIndex];
        GameManager.Instance.EnemySpawnIndex++;
        spawnPoint.currentEnemy = this;
        spawnPoint.currentAnimal = null;
        transform.position = spawnPoint.transform.position;
        Destroy(spawnPoint.animalSpawnPoint);
        if (UIManager.Instance.availableSpawnPoints.Count>0 && UIManager.Instance.availableSpawnPoints[GameManager.Instance.EnemySpawnIndex])
        {
            UIManager.Instance.availableSpawnPoints.RemoveAt(GameManager.Instance.EnemySpawnIndex);
        }
    }
    
    public event Action OnEnemyDestroy;

    public virtual void OnDeActive()
    {
        if (OnEnemyDestroy != null)
        {
            isDestroyOrThrough = true;
            OnEnemyDestroy();
            OnEnemyDestroy = null;
        }
    }

    public event Action OnDequeue;
    public virtual void OnActiveDequeue()
    {
        if (OnDequeue != null)
        {
            isDestroyOrThrough = true;
            OnDequeue();
            OnDequeue = null;
        }
    }
}
