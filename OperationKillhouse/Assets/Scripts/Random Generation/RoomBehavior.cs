using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour  
{                                          
    Generator generator;
    public Transform doorPoint;
    public bool firstRoom;
    public LayerMask doorPointLayer;
                                            
    public GameObject spawnedRoom;
    public GameObject roomRotPoint;
                                            
    public  List<Transform> doorPoints;
    public bool roomSpawned;
    public bool roomChecked;
    public bool outSideChecked;

    [Header("TestingStuf")]
    public TypeRoom room;

 
    public Transform outSideCheck;
    bool canSpawn = true;
    public Vector3 checkOffset;

    [System.Serializable]
    public class TypeRoom
    {
        public enum Room
        {
            TypeA,
            TypeB,
            TypeC,
            TypeD
        }

        public Room roomType;

        public Vector3 offset;
    }



    private void Start()
    {
        generator = GameObject.FindGameObjectWithTag("Generator").GetComponent<Generator>();
    }

    private void Update()
    {
        if(doorPoint == null)
        {
            doorPoint = GameObject.FindGameObjectWithTag("DoorPoint").transform;
        }
    
        if (doorPoint != null && !roomSpawned)
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
        {
            generator.SpawnLastRoom();
        }
    }

    public void SpawnRoom()
    {
        if(spawnedRoom == null)
            spawnedRoom = generator.rooms[Random.Range(0, generator.rooms.Length)].gameObject;

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
        room.offset = spawnedRoom.GetComponent<RoomBehavior>().checkOffset;
        Collider[] checkColl = Physics.OverlapBox(outSideCheck.position, new Vector3(17, 1, 9.5f),outSideCheck.rotation);
        VisualiseBox.DisplayBox(outSideCheck.position, new Vector3(17,1,9.5f),outSideCheck.rotation);
        if (checkColl.Length > 0)
        {
            Debug.LogError("no Room to spawn");
            generator.removeLastRoom = true;
        }

        if(checkColl.Length == 0)
        {
            generator.currentRetryAmount = generator.retryAmount;
            outSideChecked = true;
        }
    }
    
}