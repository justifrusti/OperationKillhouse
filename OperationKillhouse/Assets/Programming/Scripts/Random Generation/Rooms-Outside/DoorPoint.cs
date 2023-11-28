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
            if (!roomSpawned && generator.roomAmount > 0 && !roomManager.getLastRoom())
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
            roomManager.spawingDone = true;
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
            int roomSelect = Random.Range(0, 3);

            if (roomSelect == 0 || roomSelect == 1)
            {
                roomManager.spawnedRoom = generator.straightRooms[Random.Range(0, generator.straightRooms.Length)].gameObject;
                roomManager.spawnedCollCheck = Instantiate(roomManager.spawnedRoom.GetComponent<RoomManager>().collisionCheckOBJ, transform.position, transform.rotation);
                roomManager.checkSpawned = true;
            }

            if (roomSelect == 2)
            {
                roomManager.spawnedRoom = generator.cornerRooms[Random.Range(0, generator.cornerRooms.Length)].gameObject;
                roomManager.spawnedCollCheck = Instantiate(roomManager.spawnedRoom.GetComponent<RoomManager>().collisionCheckOBJ, transform.position, transform.rotation);
                roomManager.checkSpawned = true;
            }
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
        print("1");
        if (roomManager.spawnDoorPoint != null && roomManager.spawnedRoom == null && roomManager.spawnedCollCheck == null)
        {
            roomManager.spawnedRoom = roomManager.generator.lastRoom;
            roomManager.spawnedCollCheck = Instantiate(roomManager.spawnedRoom.GetComponent<EndRoomInfo>().collisionCheckOBJ, roomManager.spawnDoorPoint.position, roomManager.spawnDoorPoint.rotation);
        }

        if (roomManager.spawnedCollCheck != null && roomManager.spawnedCollCheck.GetComponent<CollisionCheck>().GetCollEnabled() && roomManager.spawnedCollCheck.GetComponent<CollisionCheck>().GetCollLenght() == 0)
        {
            Destroy(roomManager.spawnedCollCheck);
        }
        /*else
        {
            roomManager.SetLastRoom(false);
            generator.removeLastRoom = true;
        }*/
        
        if (roomManager.spawnedRoom != null && roomManager.spawnedRoom != null && roomManager.spawnedRoom == generator.lastRoom)
        {
            Destroy(roomManager.spawnedCollCheck);
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