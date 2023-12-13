using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour  
{                                          
    [Tooltip("Set to true if this is the start for the Dungeon")]public bool firstRoom;
    [Tooltip("The layer your doorPoints are on")]public LayerMask doorPointLayer;
    [Tooltip("All the door points in a room")]public List<Transform> doorPoints;
    [Tooltip("The centerpont of the room arount wich it can rotate")]public GameObject roomRotPoint;
    [Tooltip("Set to true if you want to spawn a specific room after this one everytime")]public bool useSetRoom;
    [ConditionalHide("useSetRoom"),Tooltip("this room will always spawn after this room")]public GameObject setRoom;
                                            
    /*[HideInInspector]*/public Generator generator;
    [HideInInspector]public GameObject spawnedRoom;                                      

    [Header("misc info")]
    public GameObject collisionCheckOBJ;
    public Transform spawnDoorPoint;
    GameObject spawnedCollCheck;
    bool checkSpawned;
    bool spawingDone;
    bool collIsClear;


    private void Start()
    {
        generator = Generator.Instance;
        spawnDoorPoint = GetComponentInChildren<DoorPoint>().transform;
        
        if (useSetRoom)
        {
            spawnedRoom = setRoom;
            spawnedCollCheck = Instantiate(spawnedRoom.GetComponent<RoomManager>().collisionCheckOBJ, spawnDoorPoint.position, spawnDoorPoint.rotation);
            checkSpawned = true;
            doorPoints.Add(spawnDoorPoint);
        }

        if (firstRoom)
        {
            spawnedRoom = generator.straightRooms[Random.Range(0, generator.straightRooms.Length)].gameObject;
            spawnedCollCheck = Instantiate(spawnedRoom.GetComponent<RoomManager>().collisionCheckOBJ, spawnDoorPoint.position, spawnDoorPoint.rotation);
            checkSpawned = true;
            doorPoints.Add(spawnDoorPoint);
        }

    }

    private void Update()
    {
        if (spawnDoorPoint == null && !generator.dungeonGenerationComplete)
        {
            Collider[] points = Physics.OverlapBox(roomRotPoint.transform.position, new Vector3(40, 1, 40), roomRotPoint.transform.rotation, doorPointLayer, queryTriggerInteraction: QueryTriggerInteraction.UseGlobal);

            for(int i = 0; i < points.Length; i++)
            {
                if (!doorPoints.Contains(points[i].transform))
                {
                    doorPoints.Add(points[i].transform);
                }
            }
            
            if(doorPoints.Count > 0)
            {
                spawnDoorPoint = doorPoints[0].transform;
            }
        }

        if (!spawnDoorPoint.GetComponent<DoorPoint>().outSideChecked)
        {
            checkColl();
        }

        if (spawingDone && doorPoints.Count < 3)
        {
            for (int i = 0; i < doorPoints.Count; i++)
            {
                if (doorPoints[i].gameObject.activeSelf == false && doorPoints.Count > 0)
                {
                    doorPoints.RemoveAt(i);
                }

                if (doorPoints.Count != 0 && doorPoints[i] == null)
                {
                    doorPoints.RemoveAt(i);
                }
            }

            if (doorPoints.Count == 0 && spawnedRoom != null)
            {
                GetComponent<RoomManager>().enabled = false;

                if (spawnedRoom.TryGetComponent<RoomManager>(out RoomManager room))
                {
                    room.enabled = true;
                }
            }
        }

        if (doorPoints.Count > 0 && doorPoints[0] == null)
        {
            doorPoints.RemoveAt(0);
        }

        if(spawnDoorPoint != null && doorPoints.Count == 0)
        {
            doorPoints.Add(spawnDoorPoint);
        }
    }

    public void checkColl()
    {
        if (checkSpawned)
        {
            if (spawnedCollCheck != null && spawnedCollCheck.GetComponent<CollisionCheck>().GetCollEnabled())
            {

                if(doorPoints.Count > 0 && spawnedCollCheck.GetComponent<CollisionCheck>().GetCollClear())
                {
                    collIsClear = true;
                    Destroy(spawnedCollCheck);
                }
                else if(spawnedCollCheck.GetComponent<CollisionCheck>().GetCollLenght() > 0)
                {
                    generator.removeLastRoom = true;
                    Destroy(spawnedCollCheck);
                }
            }

            if (spawnedCollCheck == null)
            {
                if (collIsClear)
                {
                    generator.currentRetryAmount = generator.retryAmount;
                    doorPoints[0].GetComponent<DoorPoint>().outSideChecked = true;
                    doorPoints[0].GetComponent<DoorPoint>().enabled = true;
                }
            }
        }

    }

    public void RoomReset()
    {
        spawnDoorPoint.GetComponent<DoorPoint>().ResetDoor();
        spawnedRoom = null;
        checkSpawned = false;
        spawingDone = false;
        enabled = true;
    }

    //Setter\\

    public void SetCheckSpawn(bool SpanwedCheck)
    {
        checkSpawned = SpanwedCheck;
    }

    public void SetSpawningDone(bool doneSpawning)
    {
        spawingDone = doneSpawning;
    }

    public void SetSpawnedCollCheck(GameObject collCheck)
    {
        spawnedCollCheck = collCheck;
    }

    //getters\\

    public GameObject GetSpawnedCollCheck()
    {
        return spawnedCollCheck;
    }
}