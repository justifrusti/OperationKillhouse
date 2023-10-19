using Unity.VisualScripting;
using UnityEngine;

public class DoorPoint : MonoBehaviour
{
    public Transform otherDoorPoint;
    public RoomManager roomManager;
    public Generator generator;

    public Transform outSideCheck;
    public bool roomSpawned;
    public bool roomChecked;
    public bool outSideChecked;

    private void Start()
    {
        generator = GameObject.FindGameObjectWithTag("Generator").GetComponent<Generator>();
        roomManager = GetComponentInParent<RoomManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (generator.canGenarate)
        {
            if(!roomSpawned && generator.roomAmount >= 1)
            {
                SpawnRoom();
            }
            else
            {
                Collider[] points = Physics.OverlapBox(transform.position, new Vector3(1, 1, 1), transform.rotation, roomManager.doorPointLayer, queryTriggerInteraction: QueryTriggerInteraction.UseGlobal);

                for (int i = 0; i < points.Length; i++)
                {
                    if(otherDoorPoint == null && points[i].gameObject != this.gameObject)
                    {
                        otherDoorPoint = points[i].transform;
                    }
                }
            }

            if(otherDoorPoint == this.transform)
            {
                otherDoorPoint = null;
            }
        }

        if (otherDoorPoint != null)
            CheckRoom();

        if (generator.roomAmount == 0 && generator.dungeonRooms[generator.dungeonRooms.Count - 1].GetComponent<RoomManager>().enabled == true)
            SpawnLastRoom();

        if (roomSpawned && roomChecked && outSideChecked && roomManager.doorPoints.Count < 2)
        {
            roomManager.spawingDone = true;
            gameObject.SetActive(false);

            if(otherDoorPoint != null)
            {
                otherDoorPoint.gameObject.SetActive(false);
            }
        }
    }

    public void SpawnRoom()
    {
        if (roomManager.spawnedRoom == null)
        {
            int roomSelect = Random.Range(0, 3);
            if (roomSelect == 0 || roomSelect == 1)
            {
                roomManager.spawnedRoom = generator.straightRooms[Random.Range(0, generator.straightRooms.Length)].gameObject;
            }

            if (roomSelect == 2)
            {
                roomManager.spawnedRoom = generator.cornerRooms[Random.Range(0, generator.cornerRooms.Length)].gameObject;
            }
        }

        if (outSideChecked)
        {
            roomManager.spawnedRoom = Instantiate<GameObject>(roomManager.spawnedRoom, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);

            generator.dungeonRooms.Add(roomManager.spawnedRoom);
            generator.roomAmount--;
            roomSpawned = true;
        }
    }

    public void CheckRoom()
    {
        float dst = Vector3.Distance(transform.position, otherDoorPoint.transform.position);
        if (dst < 0.1f)
        {
            roomChecked = true;
        }
    }

    public void ResetDoor()
    {
        otherDoorPoint = null;
        roomChecked = false;
        roomSpawned = false;
        gameObject.SetActive(true);
    }

    public void SpawnLastRoom()
    {
        Collider[] collCheck = Physics.OverlapBox(generator.dungeonRooms[generator.dungeonRooms.Count - 1].GetComponent<RoomManager>().outSideCheck.position, new Vector3(6, 1, 9.8f), generator.dungeonRooms[generator.dungeonRooms.Count - 1].GetComponent<RoomManager>().outSideCheck.rotation);
        
        if (collCheck.Length == 0)
        {
            Transform doorPoint = GameObject.FindGameObjectWithTag("DoorPoint").transform;

            GameObject theLastRoom = Instantiate<GameObject>(generator.lastRoom, new Vector3(doorPoint.position.x, doorPoint.position.y, doorPoint.position.z), doorPoint.rotation);
            
            generator.dungeonRooms.Add(theLastRoom);
            generator.dungeonGenerationComplete = true;
            roomSpawned = true; roomChecked = true; outSideChecked = true; enabled = false;
            gameObject.SetActive(false);
        }
        else
        {
            generator.RemoveLastRoom();
        }
    }
}
