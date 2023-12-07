using UnityEngine;

public class DoorPoint : MonoBehaviour
{
    public Transform otherDoorPoint;
    public RoomManager roomManager;
    public Generator generator;

    public bool roomSpawned;
    public bool roomChecked;
    public bool outSideChecked;

    private void Awake()
    {
        roomManager = GetComponentInParent<RoomManager>();
    }

    void Update()
    {
        if (generator == null)
        {
            generator = GameObject.FindGameObjectWithTag("Generator").GetComponent<Generator>();
        }

        if (generator != null && generator.canGenarate)
        {
            if (!roomSpawned && generator.roomAmount > 0)
            {
                SpawnRoom();
            }
            else
            {
                Collider[] points = Physics.OverlapBox(transform.position, new Vector3(1, 1, 1), transform.rotation, roomManager.doorPointLayer, queryTriggerInteraction: QueryTriggerInteraction.UseGlobal);
                for (int i = 0; i < points.Length; i++)
                {
                    if (otherDoorPoint == null && points[i].gameObject != this.gameObject)
                    {
                        otherDoorPoint = points[i].transform;
                    }
                }
            }

            if (otherDoorPoint == this.transform)
            {
                otherDoorPoint = null;
            }

            if (otherDoorPoint != null)
                CheckRoom();

            if (roomManager.enabled == true && generator.roomAmount == 0)
            {
                SpawnLastRoom();
            }
        }

        if (roomSpawned && roomChecked && roomManager.doorPoints.Count < 2)
        {
            roomManager.SetSpawningDone(true);
            gameObject.SetActive(false);

            if (otherDoorPoint != null)
            {
                otherDoorPoint.gameObject.SetActive(false);
            }
        }
    }

    public void SpawnRoom()
    {
        if (roomManager.spawnedRoom == null)
        {
            int roomSelect = Random.Range(0, 11);

            if (roomSelect <= 5)
            {
                roomManager.spawnedRoom = generator.straightRooms[Random.Range(0, generator.straightRooms.Length)].gameObject;
                roomManager.SetSpawnedCollCheck(Instantiate(roomManager.spawnedRoom.GetComponent<RoomManager>().collisionCheckOBJ, transform.position + new Vector3(0,0.2f,0), transform.rotation));
                roomManager.SetCheckSpawn(true);
            }

            if (roomSelect <= 10 && roomSelect > 5)
            {
                roomManager.spawnedRoom = generator.cornerRooms[Random.Range(0, generator.cornerRooms.Length)].gameObject;
                roomManager.SetSpawnedCollCheck(Instantiate(roomManager.spawnedRoom.GetComponent<RoomManager>().collisionCheckOBJ, transform.position + new Vector3(0, 0.2f, 0), transform.rotation));
                roomManager.SetCheckSpawn(true);
            }
/*
            if(roomSelect == 11)
            {
                roomManager.spawnedRoom = generator.staircaseDown[Random.Range(0, generator.staircaseDown.Length)].gameObject;
                roomManager.SetSpawnedCollCheck(Instantiate(roomManager.spawnedRoom.GetComponent<RoomManager>().collisionCheckOBJ, transform.position, transform.rotation));
                roomManager.SetCheckSpawn(true);
            }
            if(roomSelect == 12)
            {
                roomManager.spawnedRoom = generator.staircaseUp[Random.Range(0, generator.staircaseUp.Length)].gameObject;
                roomManager.SetSpawnedCollCheck(Instantiate(roomManager.spawnedRoom.GetComponent<RoomManager>().collisionCheckOBJ, transform.position, transform.rotation));
                roomManager.SetCheckSpawn(true);
            }*/
        }

        if(roomManager.spawnedRoom != null)
        {
            if (outSideChecked)
            {
                roomManager.spawnedRoom = Instantiate<GameObject>(roomManager.spawnedRoom, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                generator.roomAmount--;
                generator.dungeonRooms.Add(roomManager.spawnedRoom);
                roomSpawned = true;
            }
        }
    }

    public void CheckRoom()
    {
        float dst = Vector3.Distance(transform.position, otherDoorPoint.transform.position);

        if (dst < 0.11f)
        {
            roomChecked = true;
        }
    }

    public void SpawnLastRoom()
    {
        if (roomManager.spawnDoorPoint != null && roomManager.spawnedRoom == null && roomManager.GetSpawnedCollCheck() == null)
        {
            roomManager.spawnedRoom = roomManager.generator.lastRoom;
            roomManager.SetSpawnedCollCheck(Instantiate(roomManager.spawnedRoom.GetComponent<EndRoomInfo>().collisionCheckOBJ, transform.position, transform.rotation));
            roomManager.SetCheckSpawn(true);
        }
        
        if (roomManager.spawnedRoom != null && roomManager.spawnedRoom == generator.lastRoom && outSideChecked)
        {
            Destroy(roomManager.GetSpawnedCollCheck());
            Transform doorPoint = GameObject.FindGameObjectWithTag("DoorPoint").transform;

            GameObject theLastRoom = Instantiate<GameObject>(generator.lastRoom, new Vector3(doorPoint.position.x, doorPoint.position.y, doorPoint.position.z), doorPoint.rotation);
            generator.dungeonRooms.Add(theLastRoom);

            generator.dungeonGenerationComplete = true;
            roomSpawned = true; roomChecked = true; enabled = false;

            roomManager.enabled = false;

            gameObject.SetActive(false);
        }
    }

    public void ResetDoor()
    {
        otherDoorPoint = null;
        roomChecked = false;
        roomSpawned = false;
        gameObject.SetActive(true);
    }
}