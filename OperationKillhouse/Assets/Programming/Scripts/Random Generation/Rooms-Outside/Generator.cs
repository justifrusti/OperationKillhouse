using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    public static Generator Instance { get; private set; }
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
    [HideInInspector] public int roomAmount = 1;

    [Header("Info for the generator")]
    [Space]
    [Tooltip("Allows the generation to start if true")] public bool canGenarate;
    [Space]
    [Tooltip("The GameObject you want the generation to start from")] public GameObject firstRoom;
    [Tooltip("The last room you want to spawn in to close of the killHouse")] public GameObject lastRoom;
    [Space]
    [Tooltip("The amount of time the generetor is allowed to retry the placement of a room (resets when successful)")] public int retryAmount;
    [Space]
    [Tooltip("The straight rooms you want to be used for generation")] public GameObject[] cornerRooms;
    [Tooltip("The corner rooms you want to be used for generation")] public GameObject[] straightRooms;
    public GameObject[] staircaseUp;
    public GameObject[] staircaseDown;

    [Header("Misc info")]
    public List<GameObject> dungeonRooms;
    public bool dungeonGenerationComplete;
    public bool removeLastRoom;
    public SeedManager seedManager;

    public Slider progressSlider;
    int currentRetryAmount;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DifficultySelect(difficulty);
        seedManager.setSeed();
        currentRetryAmount = retryAmount;
    }

    public void Update()
    {
        if (canGenarate)
        {
            firstRoom.GetComponent<RoomManager>().gameObject.SetActive(true);
        }

        for (int i = 0; i < dungeonRooms.Count; i++)
        {
            if (dungeonRooms[i] == null)
            {
                dungeonRooms.RemoveAt(i);
            }
        }

        if (removeLastRoom && currentRetryAmount > 0)
            RemoveLastRoom();

        if(currentRetryAmount <= 0)
        {
            seedManager.reset = true;
        }

        if (dungeonGenerationComplete && gameObject.activeSelf && canGenarate)
        {
            canGenarate = false;
        }

        progressSlider.value = roomAmount;
    }

    public void RemoveLastRoom()
    {
        if (dungeonRooms.Count > 1)
        {
            Destroy(dungeonRooms[dungeonRooms.Count - 2].GetComponent<RoomManager>().GetSpawnedCollCheck());
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

        switch (difficulty)
        {
            case Difficulty.Easy:
                roomAmount = easyRooms;
                progressSlider.maxValue = easyRooms;
                break;

            case Difficulty.Medium:
                roomAmount = normalRooms;
                progressSlider.maxValue = normalRooms;
                break;

            case Difficulty.Hard:
                roomAmount = hardRooms;
                progressSlider.maxValue = hardRooms;
                break;
        }
    }

    //getters\\

    //setters

    public void SetRetryAmount(int amount)
    {
        retryAmount = amount;
    }

    public void SetCurrentRetryAmount(int amount)
    {
        currentRetryAmount = amount;
    }
}  