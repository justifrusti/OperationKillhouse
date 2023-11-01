using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Generator : MonoBehaviour
{
    public enum Difficulty
    {
        Easy, Medium, Hard
    }

    [Header("Dificulty select(hover over most variable names for tooltips)")]
    [Space(10)]
    public Difficulty difficulty;

    [Header("The amount of rooms to generate with each dificulty")]
    [Space(10)]
    public int easyRooms;
    public int normalRooms, hardRooms;
    [HideInInspector]public int roomAmount = 0;

    [Header("Info for the generator")]
    [Space]
    [Tooltip("Allows the generation to start if true")]public bool canGenarate;
    [Space]
    [Tooltip("The GameObject you want the generation to start from")]public GameObject firstRoom;
    [Tooltip("The last room you want to spawn in to close of the killHouse")]public GameObject lastRoom;
    [Space]
    [Tooltip("The amount of time the generetor is allowed to retry the placement of a room (resets when successful)")]public int retryAmount;
    [Space]
    [Tooltip("The straight rooms you want to be used for generation")]public GameObject[] cornerRooms;
    [Tooltip("The corner rooms you want to be used for generation")] public GameObject[] straightRooms;
    [Space]
    [Tooltip("If true a new dungeon will be generated")]public bool reset;
    [HideInInspector]public int currentRetryAmount;
    [HideInInspector]public List<GameObject> dungeonRooms;
    [HideInInspector]public bool dungeonGenerationComplete;
    [HideInInspector]public bool removeLastRoom;

    [Header("Seed Value's")]
    public int dungeonSeed;
    public int fillInSeed;
    public TMP_InputField seedInputField;

    public bool useFillInSeed;

    void Start()
    {
        DifficultySelect(difficulty);
        currentRetryAmount = retryAmount;
    }

    public void Update()
    {   
        for (int i = 0; i < dungeonRooms.Count; i++)
        {
            if (dungeonRooms[i] == null)
            {
                dungeonRooms.RemoveAt(i);
            }
        }

        if(currentRetryAmount <= 0)
        {
            reset = true;
        }

        if (reset)
            ResetGenerator();

        if (removeLastRoom && currentRetryAmount > 0)
            RemoveLastRoom();

        if (Input.GetKeyDown(KeyCode.G))
            startGeneration();

        if (Input.GetKeyDown(KeyCode.O))
        {
            reset = true;
        }

    }

    public void ResetGenerator()
    {
        if(dungeonRooms.Count > 0)
        {
            for (int i = 0; i < dungeonRooms.Count; i++)
            {
                Destroy(dungeonRooms[i]);
                dungeonRooms.RemoveAt(i);
            }
        }

        if(dungeonRooms.Count == 0)
        {
            firstRoom.GetComponent<RoomManager>().RoomReset();

            DifficultySelect(difficulty);

            currentRetryAmount = retryAmount;
            dungeonGenerationComplete = false;
            firstRoom.GetComponent<RoomManager>().spawnedRoom = straightRooms[Random.Range(0, straightRooms.Length)].gameObject;
            reset = false;
        }
    }

    public void RemoveLastRoom()
    {
        if(dungeonRooms.Count > 0)
        {
            Destroy(dungeonRooms[dungeonRooms.Count -1]);
            dungeonRooms[dungeonRooms.Count - 2].GetComponent<RoomManager>().RoomReset();
        }

        roomAmount++;
        currentRetryAmount--;
        removeLastRoom = false;
    }

    public void DifficultySelect(Difficulty chosenDifficulty)
    {
        difficulty = chosenDifficulty;

        switch(difficulty)
        {
            case Difficulty.Easy:
                roomAmount = easyRooms;
                break;

            case Difficulty.Medium:
                roomAmount = normalRooms;
                break;

            case Difficulty.Hard:
                roomAmount = hardRooms;
                break;
        }
    }

    public void startGeneration()
    {
        
        if(seedInputField.text.Length > 0)
        {
            useFillInSeed = true;

            fillInSeed = int.Parse(seedInputField.text);
            dungeonSeed = fillInSeed;

            canGenarate = true;
        }

        if(seedInputField.text.Length == 0)
        {
            dungeonSeed = Random.Range(0, int.MaxValue);
            canGenarate = true;
        }
            Random.InitState(dungeonSeed);
    }
}
