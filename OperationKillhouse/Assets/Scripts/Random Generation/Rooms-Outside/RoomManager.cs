using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour  
{                                          
    [Tooltip("Set to true if this is the start for the Dungeon")]public bool firstRoom;
    [Tooltip("The layer your doorPoints are on")]public LayerMask doorPointLayer;
    [Tooltip("All the door points in a room")]public List<Transform> doorPoints;
    [Tooltip("The centerpont of the room arount wich it can rotate")]public GameObject roomRotPoint;
                                            
    [HideInInspector]public Generator generator;
    [HideInInspector]public GameObject spawnedRoom;                                      

    [Header("misc info")]
    [Tooltip("The centerpoint of the check if a room doesn't collide with annything when spawned")]public Transform outSideCheck;
    [HideInInspector]public bool spawingDone;

    
    public Transform spawnDoorPoint;

    private void Start()
    {
        generator = GameObject.FindGameObjectWithTag("Generator").GetComponent<Generator>();
        
        if (firstRoom)
        {
            spawnedRoom = generator.straightRooms[Random.Range(0, generator.straightRooms.Length)].gameObject;
        }
    }

    private void Update()
    {
        if(spawnDoorPoint == null && !generator.dungeonGenerationComplete)
        {
            Collider[] points = Physics.OverlapBox(roomRotPoint.transform.position, new Vector3(20, 1, 20), roomRotPoint.transform.rotation, doorPointLayer, queryTriggerInteraction: QueryTriggerInteraction.UseGlobal);

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

        if(!spawnDoorPoint.GetComponent<DoorPoint>().outSideChecked && spawnedRoom != null)
            checkColl();

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

    }

    public void checkColl()
    {

        Collider[] checkColl = Physics.OverlapBox(outSideCheck.position, new Vector3(18, 1, 9.8f),outSideCheck.rotation);

        if (checkColl.Length > 0)
        {
            generator.removeLastRoom = true;
        }

        if(checkColl.Length == 0 && doorPoints.Count > 0)
        {
            generator.currentRetryAmount = generator.retryAmount;
            doorPoints[0].GetComponent<DoorPoint>().enabled = true;
            doorPoints[0].GetComponent<DoorPoint>().outSideChecked = true;
        }
    }

    public void RoomReset()
    {
        spawnDoorPoint.GetComponent<DoorPoint>().ResetDoor();
        spawnedRoom = null;
        enabled = true;
    }
}