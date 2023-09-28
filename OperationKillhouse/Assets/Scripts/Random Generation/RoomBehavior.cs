using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    Generator generator;
    public Transform doorPoint;
    public bool roomSpawned;
    public bool roomChecked;
    public bool firstRoom;

    public List<Transform> doorPoints;

    public LayerMask doorPointLayer;

    public GameObject spawnedRoom;
    public GameObject roomRotPoint;
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
                SpawnRoom();
            else if(doorPoints.Count == 0)
                CheckForDoorPoints();
        }

        if (roomSpawned && !roomChecked)
            CheckRoom();
        
        if (roomSpawned && roomChecked && doorPoints.Count < 3)
        {
            for (int i = 0; i < doorPoints.Count; i++)
            {
                Destroy(doorPoints[i].gameObject);
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
        spawnedRoom = generator.rooms[Random.Range(0, generator.rooms.Length)].gameObject;

        spawnedRoom = Instantiate<GameObject>(spawnedRoom, new Vector3(doorPoint.position.x, doorPoint.position.y, doorPoint.position.z), doorPoint.rotation);

        generator.roomAmount--;
        CheckForDoorPoints();
        roomSpawned = true;

    }

    public void CheckRoom()
    {
        if (doorPoints.Count == 2)
        {
            float dst = Vector3.Distance(doorPoints[0].transform.position, doorPoints[1].transform.position);
            print(dst);
            if(dst < .1005f)
            {
                roomChecked = true;
            }
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
}
/*
 * 
 * 
 * 
 * 
*/