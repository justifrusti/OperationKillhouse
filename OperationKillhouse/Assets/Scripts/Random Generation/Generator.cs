using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Dificulty select (ONLY SELECT ONE AT A TIME)")]
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
    public GameObject lastRoom;
    public int retryAmount;
    [HideInInspector]public int currentRetryAmount;
    [Tooltip("The rooms you want to be used for generation")]public GameObject[] rooms;
    [HideInInspector]public List<GameObject> killHouseRooms;
    [HideInInspector] public bool killHouseGenerationComplete;
    [Tooltip("If true a new dungeon will be generated")]public bool reset;
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
        {
            ResetGenerator();
        }
        
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
            firstRoom.GetComponent<RoomBehavior>().spawnedRoom = null;
            firstRoom.GetComponent<RoomBehavior>().roomChecked = false;
            firstRoom.GetComponent<RoomBehavior>().roomSpawned = false;
            firstRoom.GetComponent<RoomBehavior>().doorPoint.gameObject.SetActive(true);
            firstRoom.GetComponent<RoomBehavior>().enabled = true;

            if (easy)
                roomAmount = easyRooms;

            if (normal)
                roomAmount = normalRooms;

            if (hard)
                roomAmount = hardRooms;
            currentRetryAmount = retryAmount;
            killHouseGenerationComplete = false;
            reset = false;
        }
    }

    public void RemoveLastRoom()
    {
        Destroy(killHouseRooms[killHouseRooms.Count -1]);
        killHouseRooms[killHouseRooms.Count - 2].GetComponent<RoomBehavior>().spawnedRoom = null;
        killHouseRooms[killHouseRooms.Count - 2].GetComponent<RoomBehavior>().roomChecked = false;
        killHouseRooms[killHouseRooms.Count - 2].GetComponent<RoomBehavior>().roomSpawned = false;
        killHouseRooms[killHouseRooms.Count - 2].GetComponent<RoomBehavior>().doorPoint.gameObject.SetActive(true);
        killHouseRooms[killHouseRooms.Count - 2].GetComponent<RoomBehavior>().enabled = true;
        roomAmount++;
        currentRetryAmount--;
        removeLastRoom = false;
    }

    public void SpawnLastRoom()
    { 
        Transform doorPoint = GameObject.FindGameObjectWithTag("DoorPoint").transform;
        GameObject theLastRoom = Instantiate<GameObject>(lastRoom, new Vector3(doorPoint.position.x, doorPoint.position.y, doorPoint.position.z), doorPoint.rotation);
        doorPoint.gameObject.SetActive(false);
        killHouseRooms.Add(theLastRoom);
        killHouseGenerationComplete = true;
    }
}
