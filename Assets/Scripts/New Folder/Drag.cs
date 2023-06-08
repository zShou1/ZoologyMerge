using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private Vector3 mOffset;

    private float mZCoord;

    private Vector3 beforePosition;

    private SpawnPoint beforeSpawnPoint;

    public bool isOnLane = false;

    private SpawnPoint currentSpawnPointLight = null;

    private Lane currentLaneLight = null;
    private void Start()
    {
    }

    private void OnEnable()
    {
        CreateInfoAnimalOnSpawnPoint();
        DeactiveDragOnLane();
    }

    private void OnMouseDown()
    {
        if (isOnLane) return;
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        /*mousePoint.z = gameObject.transform.position.z;*/
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        if (isOnLane) return;
        Vector3 pos = GetMouseWorldPos() + mOffset;
        transform.position = new Vector3(pos.x, 0.5f, pos.z);
    }

    private void OnMouseUp()
    {
        if (isOnLane) return;
        /*Debug.Log("2132");*/
        CheckBellow();
    }

    private void OnEnableSpawnPointLight()
    {
        //Doan nay can sua?
        
            RaycastHit hitInfo;
            /*Debug.DrawLine(transform.position, transform.position + Vector3.down * 3f, Color.red);*/
            if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 3f, LayerMask.GetMask("SpawnPoint"),
                QueryTriggerInteraction.Collide))
            {
                // Raycast trúng SpawnPoint mới
                SpawnPoint newSpawnPointLight = hitInfo.collider.GetComponent<SpawnPoint>();

                if (newSpawnPointLight != null)
                {
                    if (currentSpawnPointLight != null)
                    {
                        currentSpawnPointLight.DeActiveLight();
                    }
                    // Bật sáng SpawnPoint mới
                    newSpawnPointLight.OnActiveLight();

                    // Cập nhật SpawnPoint hiện tại
                    currentSpawnPointLight = newSpawnPointLight;
                }
            }
            else
            {
                if (currentSpawnPointLight != null)
                {
                    currentSpawnPointLight.DeActiveLight();
                }

                // Đặt SpawnPoint hiện tại về null
                    currentSpawnPointLight = null;
                
            }
    }

    private void OnEnableLaneLight()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 3f, LayerMask.GetMask("Lane"),
            QueryTriggerInteraction.Collide))
        {
            Lane newLaneLight = hitInfo.collider.GetComponent<Lane>();
            if (newLaneLight != null)
            {
                if (currentLaneLight != null)
                {
                    currentLaneLight.DeActiveLaneLight();
                }
                newLaneLight.OnActiveLaneLight();
                currentLaneLight = newLaneLight;
            }
        }
        else
        {
            if (currentLaneLight != null)
            {
                currentLaneLight.DeActiveLaneLight();
            }
            currentLaneLight = null;
        }
    }

    private void Update()
    {
        OnEnableSpawnPointLight();
        OnEnableLaneLight();
    }

    void DeactiveDragOnLane()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hitInfo, 3f,
            LayerMask.GetMask("Lane"),
            QueryTriggerInteraction.Collide))
        {
            Lane lane = hitInfo.collider.GetComponent<Lane>();
            Animal thisAnimal = gameObject.GetComponent<Animal>();
            if (hitInfo.collider.transform != null)
            {
                lane.currentAnimalOnLane = thisAnimal;
                lane.animalOnLane = thisAnimal.gameObject;
                isOnLane = true;
                thisAnimal.animalOnLanePosition = thisAnimal.transform.position;
                lane.DeActiveLaneLight();
                currentLaneLight = null;
            }
        }
    }

    private void CreateInfoAnimalOnSpawnPoint()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hitInfo, 3f,
            LayerMask.GetMask("SpawnPoint"),
            QueryTriggerInteraction.Collide))
        {
            SpawnPoint spawnPoint = hitInfo.collider.GetComponent<SpawnPoint>();
            Animal thisAnimal = gameObject.GetComponent<Animal>();
            if (hitInfo.collider.transform != null)
            {
                if (beforeSpawnPoint == null)
                {
                    beforeSpawnPoint = spawnPoint;
                }

                if (spawnPoint.currentAnimal == null)
                {
                    spawnPoint.currentAnimal = thisAnimal;
                }

                spawnPoint.animalSpawnPoint = thisAnimal.gameObject;
                spawnPoint.DeActiveLight();
                currentSpawnPointLight = null;
            }
        }
    }

    private void CheckBellow()
    {
        Animal thisAnimal = gameObject.GetComponent<Animal>();
        RaycastHit hitInfo;
        RaycastHit hitLaneInfo;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hitInfo, 3f,
            LayerMask.GetMask("SpawnPoint"),
            QueryTriggerInteraction.Collide))
        {
            SpawnPoint spawnPoint = hitInfo.collider.GetComponent<SpawnPoint>();
            if (hitInfo.collider.transform != null && spawnPoint.currentAnimal == null &&
                spawnPoint.currentEnemy == null)
            {
                if (spawnPoint.currentAnimal == null)
                {
                    spawnPoint.currentAnimal = thisAnimal;
                }

                spawnPoint.animalSpawnPoint = thisAnimal.gameObject;
                beforeSpawnPoint.currentAnimal = null;
                beforeSpawnPoint.animalSpawnPoint = null;
                transform.position = hitInfo.collider.transform.position;
                beforeSpawnPoint = spawnPoint;
                /*beforePosition = transform.position;*/
            }
            else if (spawnPoint.currentEnemy == null &&
                     thisAnimal.animalLevel == spawnPoint.currentAnimal.animalLevel && spawnPoint != beforeSpawnPoint)
            {
                beforeSpawnPoint.currentAnimal = null;
                beforeSpawnPoint.animalSpawnPoint = null;
                Destroy(spawnPoint.animalSpawnPoint);
                Destroy(gameObject);
                Animal animalMerge = UIManager.Instance.listAnimals.Find(animal =>
                    animal.animalLevel == (thisAnimal.animalLevel + 1));
                if (animalMerge.animalLevel > GameManager.Instance.LevelMaxAnimal)
                {
                    GameManager.Instance.LevelMaxAnimal = animalMerge.animalLevel;
                    UIManager.Instance.SetMaxLevelAnimalPanel();
                }

                /*Instantiate(animalMerge.gameObject, spawnPoint.transform.position, Quaternion.identity);
                Debug.Log(PrefabUtility.IsPartOfPrefabAsset(animalMerge));*/
                GameObject newAnimal = Instantiate(animalMerge.gameObject, spawnPoint.transform.position,
                    Quaternion.identity);

                spawnPoint.currentAnimal = animalMerge;
            }
            else
            {
                beforeSpawnPoint.currentAnimal = thisAnimal;
                beforeSpawnPoint.animalSpawnPoint = thisAnimal.gameObject;
                transform.position = beforeSpawnPoint.transform.position;
                /*transform.position = beforePosition;*/
            }
        }
        else if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hitLaneInfo, 3f,
            LayerMask.GetMask("Lane"),
            QueryTriggerInteraction.Collide))
        {
            Lane lane = hitLaneInfo.collider.GetComponent<Lane>();
            Vector3 animalOnLanePosition = hitLaneInfo.collider.transform.position -
                                           new Vector3(0, 0, lane.laneHeight / 2) + new Vector3(0, 0, 0.2f);
            if (hitLaneInfo.collider.transform != null && lane.currentAnimalOnLane == null)
            {
                if (lane.currentAnimalOnLane == null)
                {
                    lane.currentAnimalOnLane = thisAnimal;
                }

                lane.animalOnLane = thisAnimal.gameObject;
                beforeSpawnPoint.currentAnimal = null;
                beforeSpawnPoint.animalSpawnPoint = null;
                transform.position = animalOnLanePosition;
                isOnLane = true;
                thisAnimal.animalOnLanePosition = animalOnLanePosition;
            }
            else if (thisAnimal.animalLevel == lane.currentAnimalOnLane.animalLevel)
            {
                AnimalState currentAnimalState = lane.currentAnimalOnLane.currentState;
                beforeSpawnPoint.currentAnimal = null;
                beforeSpawnPoint.animalSpawnPoint = null;
                
                Animal animalMerge = UIManager.Instance.listAnimals.Find(animal =>
                    animal.animalLevel == (thisAnimal.animalLevel + 1));
                Destroy(lane.animalOnLane);
                Destroy(gameObject);
                if (animalMerge.animalLevel > GameManager.Instance.LevelMaxAnimal)
                {
                    GameManager.Instance.LevelMaxAnimal = animalMerge.animalLevel;
                    UIManager.Instance.SetMaxLevelAnimalPanel();
                }

                /*Instantiate(animalMerge.gameObject, spawnPoint.transform.position, Quaternion.identity);
                Debug.Log(PrefabUtility.IsPartOfPrefabAsset(animalMerge));*/
                GameObject newAnimal = Instantiate(animalMerge.gameObject, /*animalOnLanePosition*/ lane.animalOnLane.transform.position,
                    Quaternion.identity);

                Animal animal = newAnimal.GetComponent<Animal>();
                animal.animalOnLanePosition = animalOnLanePosition;
                animal.SetState(currentAnimalState);
                /*lane.currentAnimalOnLane = animalMerge;
                lane.animalOnLane = newAnimal;*/
                /*spawnPoint.currentAnimal = animalMerge;*/
            }
            else if (thisAnimal.animalLevel > lane.currentAnimalOnLane.animalLevel)
            {
                thisAnimal.animalOnLanePosition = animalOnLanePosition;
                transform.position = /*animalOnLanePosition*/ lane.animalOnLane.transform.position;
                thisAnimal.SetState(lane.currentAnimalOnLane.currentState);
                Destroy(lane.animalOnLane);
                lane.currentAnimalOnLane = thisAnimal;
                lane.animalOnLane = thisAnimal.gameObject;
                beforeSpawnPoint.currentAnimal = null;
                beforeSpawnPoint.animalSpawnPoint = null;
                isOnLane = true;
                
                
            }
            else
            {
                beforeSpawnPoint.currentAnimal = thisAnimal;
                beforeSpawnPoint.animalSpawnPoint = thisAnimal.gameObject;
                transform.position = beforeSpawnPoint.transform.position;
                /*transform.position = beforePosition;*/
            }
        }
        else
        {
            /*Debug.Log("aaa");*/
            transform.position = beforeSpawnPoint.transform.position;
        }
    }
}