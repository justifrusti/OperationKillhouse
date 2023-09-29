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
    [HideInInspector]public bool roomSpawned;
    [HideInInspector] public bool roomChecked;


    [Header("TestingStuf")]
    public bool aRoom;
    public bool bRoom, cRoom, dRoom;
    bool canSpawn = true;
    Vector3 offset;
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
        
        if (roomSpawned && roomChecked && doorPoints.Count < 3)
        {
            for (int i = 0; i < doorPoints.Count; i++)
            {
                doorPoints[i].gameObject.SetActive(false);
                doorPoints.Remove(doorPoints[i]);
            }

            if (doorPoints.Count == 0)
            {
                spawnedRoom.GetComponent<RoomBehavior>().enabled = true;
                GetComponent<RoomBehavior>().enabled = false;
            }
        }
    }

    public void SpawnRoom()
    {
        if(spawnedRoom == null)
            spawnedRoom = generator.rooms[Random.Range(0, generator.rooms.Length)].gameObject;



        spawnedRoom = Instantiate<GameObject>(spawnedRoom, new Vector3(doorPoint.position.x, doorPoint.position.y, doorPoint.position.z), doorPoint.rotation);
        
        generator.killHouseRooms.Add(spawnedRoom);
        generator.roomAmount--;
        CheckForDoorPoints();
        roomSpawned = true;

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
}/*
  *  if (aRoom)
            offset = doorPoint.position;

        Collider[] checkColl = Physics.OverlapBox(doorPoint.position + offset, new Vector3(9, 1, 9),doorPoint.rotation);

        if(checkColl.Length > 0)
        {
            canSpawn = false;
            for (int i = 0;i < checkColl.Length;i++)
            {
                Debug.Log(checkColl[i]);
            }
        }
  */