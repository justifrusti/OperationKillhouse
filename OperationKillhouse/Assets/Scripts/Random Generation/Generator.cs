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
    [Tooltip("The rooms you want to be used for generation")]public GameObject[] rooms;
    [Tooltip("The GameObject you want the generation to start from")]public GameObject firstRoom;
    [HideInInspector]public List<GameObject> killHouseRooms;

    [Tooltip("If true a new dungeon will be generated")]public bool reset;

    // Start is called before the first frame update
    void Start()
    {
        if (easy)
            roomAmount = easyRooms;

        if (normal)
            roomAmount = normalRooms;

        if(hard)
            roomAmount = hardRooms;

    }

    public void Update()
    {
        if (reset)
        {
            ResetGenerator();
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
            firstRoom.GetComponent<RoomBehavior>().enabled = true;
            firstRoom.GetComponent<RoomBehavior>().roomChecked = false;
            firstRoom.GetComponent<RoomBehavior>().roomSpawned = false;
            firstRoom.GetComponent<RoomBehavior>().doorPoint.gameObject.SetActive(true);
            if (easy)
                roomAmount = easyRooms;

            if (normal)
                roomAmount = normalRooms;

            if (hard)
                roomAmount = hardRooms;
            reset = false;
        }
    }
}
