using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Dificulty select")]
    [Space(10)]
    public bool easy;
    public bool normal, hard;

    [Header("The amount of rooms to generate with each dificulty")]
    [Space(10)]
    public int easyRooms;
    public int normalRooms, hardRooms;
    [HideInInspector]public int roomAmount = 0;
    [Space(10)]
    [Tooltip("The rooms you want to be used for generation")]public GameObject[] rooms;

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
}
