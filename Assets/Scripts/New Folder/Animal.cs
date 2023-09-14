using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public Sprite animalSprite;

    public int animalLevel;

    public float timeCountdown;

    public int weightRoll;

    public Vector3 animalOnLanePosition;

    private Rigidbody rb;

    private float speed = 6f;

    public float deltaDistance = 0.5f;

    public bool isInitMoveToPlayer = false;

    public float zLocalScale;

    public bool isAttacked = false;

    public bool isMoved = false;

    private Animator animator;

    public AnimalState currentState;

    public Transform currentEnemyAttackTransform;

    public bool isAttacking = false;

    private void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        zLocalScale = transform.localScale.z;
        currentState = AnimalState.Freeze;
        transform.localScale= new Vector3(0.08f, 0.08f, 0.08f);
        transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f);
    }

    private void OnEnable()
    {
        isInitMoveToPlayer = false;
        isAttacked = false;
        /*UpdateState();*/
    }

    private void Start()
    {
    }

    public void SetState(AnimalState newState)
    {
        currentState = newState;
    }
/*    public IEnumerator MoveToEnemy(Transform enemyTransform)
    {
        //- Lỗi ở đây, State Attack đang bị đè bằng 1 State khác liên tục
        isInitMoveToPlayer = true;
        rb.isKinematic = false;
        rb.velocity = speed * Time.fixedDeltaTime * Vector3.forward;
        if (Vector3.Magnitude(enemyTransform.position - transform.position) <= deltaDistance)
        {
            rb.velocity = Vector3.zero;
            //Code fix tạm thời
            Attack();
            /*currentState = AnimalState.Attack;#1#
            /*SetState(AnimalState.Attack);#1#
        }
        yield return new WaitUntil(()=>isInitMoveToPlayer&&isAttacked);

    }*/

    public void MoveToEnemy(Transform enemyTransform)
    {
        //- Lỗi ở đây, State Attack đang bị đè bằng 1 State khác liên tục
        isInitMoveToPlayer = true;
        rb.isKinematic = false;
        rb.velocity = speed * Time.fixedDeltaTime * Vector3.forward;
/*        if (Vector3.Magnitude(enemyTransform.position - transform.position) <= deltaDistance)
        {
            rb.velocity = Vector3.zero;
            //Code fix tạm thời
            /*Attack();#1#
            /*currentState = AnimalState.Attack;#1#
            SetState(AnimalState.Attack);
        }*/
    }

    public void MoveToAnimalOnLanePosition()
    {
        rb.isKinematic = false;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -zLocalScale);
        rb.velocity = -1 * speed * Time.fixedDeltaTime * Vector3.forward;
        if (transform.position.z <= animalOnLanePosition.z)
        {
            currentState = AnimalState.Freeze;
            transform.position = animalOnLanePosition;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, zLocalScale);
        }
    }

    private void Update()
    {
        /*Debug.Log(isAttacked);
        Debug.Log(currentState);*/
        UpdateState();
        /*Debug.Log(gameObject.name+ " "+ rb.velocity.z);*/
    }

    void UpdateState()
    {
        animator.SetFloat("Velocity", Math.Abs(rb.velocity.z));
        switch (currentState)
        {
            default:
                break;
            case AnimalState.Freeze:
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, zLocalScale);
                break;
            case AnimalState.Attack:
                if (!isAttacked)
                {
                    Attack();
                }

                break;
            case AnimalState.MoveToEnemy:
                /*StartCoroutine(MoveToEnemy(currentEnemyAttackTransform));*/
                MoveToEnemy(currentEnemyAttackTransform);
                break;
            case AnimalState.MoveToLanePosition:
                MoveToAnimalOnLanePosition();
                break;
        }
    }


    public void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        isAttacked = true;
        isAttacking = false;
    }


    /*private IEnumerator Attack()
    {
        isAttacking = true;
        rb.velocity = Vector3.zero;
        Debug.Log("agagag");
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.2f);
        isAttacked = true;
        isAttacking = false;
        yield return new WaitUntil(()=>isAttacked==false);
    }*/
}