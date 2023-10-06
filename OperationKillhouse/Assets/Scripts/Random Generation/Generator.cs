using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Dificulty select(hover over most variable names for tooltips)")]
    [Space(10)]
    public bool easy;
    public bool normal, hard;

    [Header("The amount of rooms to generate with each dificulty")]
    [Space(10)]
    public int easyRooms;
    public int normalRooms, hardRooms;
    [HideInInspector]public int roomAmount = 0;

    [Header("Info for the generator")]
    [Space(10)]
    [Tooltip("The GameObject you want the generation to start from")]public GameObject firstRoom;
    [Tooltip("The last room you want to spawn in to close of the killHouse")]public GameObject lastRoom;
    [Tooltip("The amount of time the generetor is allowed to retry the placement of a room (resets when successful)")]public int retryAmount;
    [Tooltip("The rooms you want to be used for generation")]public GameObject[] cornerRooms;
    [Tooltip("The rooms you want to be used for generation")] public GameObject[] straightRooms;
    [Tooltip("If true a new dungeon will be generated")]public bool reset;
    [HideInInspector]public int currentRetryAmount;
    [HideInInspector]public List<GameObject> killHouseRooms;
    [HideInInspector] public bool killHouseGenerationComplete;
    [HideInInspector]public bool removeLastRoom;

    // S4tart is called before the first frame update
    void Start()
    {
        if (easy)
            roomAmount = easyRooms;

        if (normal)
            roomAmount = normalRooms;

        if(hard)
            roomAmount = hardRooms;

        currentRetryAmount = retryAmount;
    }

    public void Update()
    {
        if (reset)
            ResetGenerator();
        
        if(removeLastRoom && currentRetryAmount > 0)
            RemoveLastRoom();

        for (int i = 0; i < killHouseRooms.Count; i++)
        {
            if (killHouseRooms[i] == null)
            {
                killHouseRooms.RemoveAt(i);
            }
        }

        if(currentRetryAmount <= 0)
        {
            reset = true;
        }
    }

    public void ResetGenerator()
    {
        if(killHouseRooms.Count > 0)
        {
            for (int i = 0; i < killHouseRooms.Count; i++)
            {
                Destroy(killHouseRooms[i]);
                killHouseRooms.RemoveAt(i);
            }
        }

        if(killHouseRooms.Count == 0)
        {
            firstRoom.GetComponent<RoomBehavior>().RoomReset();

            if (easy)
                roomAmount = easyRooms;

            if (normal)
                roomAmount = normalRooms;

            if (hard)
                roomAmount = hardRooms;

            currentRetryAmount = retryAmount;
            killHouseGenerationComplete = false;
            firstRoom.GetComponent<RoomBehavior>().spawnedRoom = straightRooms[Random.Range(0, straightRooms.Length)].gameObject;
            reset = false;
        }
    }

    public void RemoveLastRoom()
    {
        Destroy(killHouseRooms[killHouseRooms.Count -1]);
        killHouseRooms[killHouseRooms.Count - 2].GetComponent<RoomBehavior>().RoomReset();
        roomAmount++;
        currentRetryAmount--;
        removeLastRoom = false;
    }

    public void SpawnLastRoom()
    { 
        Collider[] collCheck = Physics.OverlapBox(killHouseRooms[killHouseRooms.Count -1].GetComponent<RoomBehavior>().outSideCheck.position, new Vector3(6,1,9.8f), killHouseRooms[killHouseRooms.Count - 1].GetComponent<RoomBehavior>().outSideCheck.rotation);
        VisualiseBox.DisplayBox(killHouseRooms[killHouseRooms.Count - 1].GetComponent<RoomBehavior>().outSideCheck.position, new Vector3(6, 1, 9.8f), killHouseRooms[killHouseRooms.Count - 1].GetComponent<RoomBehavior>().outSideCheck.rotation);
        if(collCheck.Length == 0)
        {
            Transform doorPoint = GameObject.FindGameObjectWithTag("DoorPoint").transform;
            GameObject theLastRoom = Instantiate<GameObject>(lastRoom, new Vector3(doorPoint.position.x, doorPoint.position.y, doorPoint.position.z), doorPoint.rotation);
            doorPoint.gameObject.SetActive(false);
            killHouseRooms.Add(theLastRoom);
            killHouseGenerationComplete = true;
        }
        else
        {
            RemoveLastRoom();
        }
    }
}
