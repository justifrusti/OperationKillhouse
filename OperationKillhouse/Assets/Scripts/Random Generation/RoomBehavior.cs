using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour  
{                                          
    Generator generator;
    private Transform doorPoint;
    [Tooltip("Set to true if this is the start for the killhouse")]public bool firstRoom;
    [Tooltip("The layer your doorPoints are on")]public LayerMask doorPointLayer;
                                            
    [HideInInspector]public GameObject spawnedRoom;                                      
    [HideInInspector]public List<Transform> doorPoints;
    private bool roomSpawned;
    private bool roomChecked;
    private bool outSideChecked;

    [Header("misc info")]
    [Tooltip("The centerpoint of the check if a room doesn't collide with annything when spawned")]public Transform outSideCheck;
    [Tooltip("The offset of the outSideCheck")]public Vector3 offset;
    private Vector3 checkOffset;

    private void Start()
    {
        generator = GameObject.FindGameObjectWithTag("Generator").GetComponent<Generator>();
        if(firstRoom)
            spawnedRoom = generator.straightRooms[Random.Range(0, generator.straightRooms.Length)].gameObject;
    }

    private void Update()
    {
        if(doorPoint == null)
            doorPoint = GameObject.FindGameObjectWithTag("DoorPoint").transform;
    
        if (doorPoint != null && !roomSpawned && generator.canGenarate)
        {
            if (generator.roomAmount > 0 && doorPoints.Count > 0)
            {
                SpawnRoom();
            }
            else if(doorPoints.Count == 0)
            {
                doorPoints.Add(doorPoint);
                CheckForDoorPoints();
            }
        }

        if (roomSpawned && !roomChecked)
            CheckRoom();

        if (!outSideChecked && spawnedRoom != null)
            checkColl();
        
        if (roomSpawned && roomChecked && outSideChecked && doorPoints.Count < 3)
        {
            for (int i = 0; i < doorPoints.Count; i++)
            {
                doorPoints[i].gameObject.SetActive(false);
                doorPoints.Remove(doorPoints[i]);
            }

            if (doorPoints.Count == 0)
            {
                GetComponent<RoomBehavior>().enabled = false;
                spawnedRoom.GetComponent<RoomBehavior>().enabled = true;
            }
        }

        if (generator.roomAmount == 0 && !generator.killHouseGenerationComplete && doorPoints.Count == 1 && generator.killHouseRooms[generator.killHouseRooms.Count -1].GetComponent<RoomBehavior>().enabled == true)
            generator.SpawnLastRoom();
    }

    public void SpawnRoom()
    {

        if(spawnedRoom == null)
        {
            int roomSelect = Random.Range(0, 2);
            if(roomSelect == 0)
            {
                spawnedRoom = generator.straightRooms[Random.Range(0, generator.straightRooms.Length)].gameObject;
            }

            if(roomSelect == 1)
            {
                spawnedRoom = generator.cornerRooms[Random.Range(0, generator.cornerRooms.Length)].gameObject;
            }
        }

        if(outSideChecked)
        {
            spawnedRoom = Instantiate<GameObject>(spawnedRoom, new Vector3(doorPoint.position.x, doorPoint.position.y, doorPoint.position.z), doorPoint.rotation);
        
            generator.killHouseRooms.Add(spawnedRoom);
            generator.roomAmount--;
            CheckForDoorPoints();
            roomSpawned = true;
        }
    }

    public void CheckRoom()
    {
        if (doorPoints.Count == 2)
        {
            float dst = Vector3.Distance(doorPoints[0].transform.position, doorPoints[1].transform.position);

            if(dst < .1005f)
                roomChecked = true;
        }
    }

    public void CheckForDoorPoints()
    {
        if (doorPoints.Count < 2)
        {
            Collider[] points = Physics.OverlapBox(doorPoint.position, new Vector3(1, .05f, 1), doorPoint.rotation, doorPointLayer, queryTriggerInteraction: QueryTriggerInteraction.UseGlobal);
            foreach (Collider c in points)
            {
                if (!doorPoints.Contains(c.transform))
                {
                    doorPoints.Add(c.transform);
                }
            }
        }
    }

    public void checkColl()
    {
        offset = spawnedRoom.GetComponent<RoomBehavior>().checkOffset;
        Collider[] checkColl = Physics.OverlapBox(outSideCheck.position, new Vector3(18, 1, 9.8f),outSideCheck.rotation);
        VisualiseBox.DisplayBox(outSideCheck.position, new Vector3(17,1,9.8f),outSideCheck.rotation);

        if (checkColl.Length > 0)
        {
            generator.removeLastRoom = true;
        }

        if(checkColl.Length == 0)
        {
            generator.currentRetryAmount = generator.retryAmount;
            outSideChecked = true;
        }
    }
    
    public void RoomReset()
    {
        spawnedRoom = null;
        roomChecked = false;
        roomSpawned = false;
        doorPoint.gameObject.SetActive(true);
        enabled = true;
    }
}