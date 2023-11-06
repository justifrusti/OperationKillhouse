using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class RoomInteriorManager : MonoBehaviour
{
    [Serializable]
    public class SpawnPoint
    {
        public Transform spawnPoint;

        public enum GenerateDirection
        {
            Right,
            Left,
            Up,
            Down
        }

        public GenerateDirection direction;
    }

    public GameObject normalWall;
    public GameObject[] doorWall;

    public SpawnPoint[] startPoint;

    public int minWalls;
    public int maxWalls;

    public float offset;

    public bool reSpawn;

    [Range(0.00f, 1.00f)]public float turnedChance;
    [Range(0.00f, 1.00f)] public float doorChance;

    int s_WallsToSpawn;
    int s_StartPointIndex = 0;

    bool s_FirstWall = true;

    bool s_SpawnedDoor = false;
    bool s_Turned = false;

    Quaternion s_RotationalPos = new Quaternion();
    Vector3 s_SpawnPoint;

    List<GameObject> walls = new List<GameObject>();

    void Start()
    {
        s_SpawnPoint = startPoint[s_StartPointIndex].spawnPoint.position;

        GenerateWalls();
    }

    private void Update()
    {
        if(s_StartPointIndex != startPoint.Length && !reSpawn)
        {
            s_SpawnedDoor = false;
            s_FirstWall = true;
            s_Turned = false;
            s_RotationalPos = new Quaternion();

            GenerateWalls();
        }
    }

    void GenerateWalls()
    {
        s_WallsToSpawn = Random.Range(minWalls, maxWalls + 1);
        s_SpawnPoint = startPoint[s_StartPointIndex].spawnPoint.position;

        GameObject go = new GameObject();

        for (int i = 0; i < s_WallsToSpawn; i++)
        {
            if (Random.value < doorChance && !s_SpawnedDoor)
            {
                s_SpawnedDoor = true;

                go = Instantiate(doorWall[Random.Range(0, doorWall.Length)], s_SpawnPoint, s_RotationalPos, startPoint[s_StartPointIndex].spawnPoint);
            }
            else if (Random.value < turnedChance && !s_FirstWall)
            {
                s_Turned = !s_Turned;

                ApplyWallRotation();

                go = Instantiate(normalWall, s_SpawnPoint, s_RotationalPos, startPoint[s_StartPointIndex].spawnPoint);
            }
            else
            {
                go = Instantiate(normalWall, s_SpawnPoint, s_RotationalPos, startPoint[s_StartPointIndex].spawnPoint);
            }

            walls.Add(go);

            if (s_FirstWall)
            {
                s_FirstWall = false;
            }

            AddOffset();
        }

        s_StartPointIndex++;
    }

    void ApplyWallRotation()
    {
        switch(startPoint[s_StartPointIndex].direction)
        {
            case SpawnPoint.GenerateDirection.Right:
                if (s_Turned)
                {
                    s_RotationalPos = Quaternion.Euler(0, -90, 0);
                }
                else
                {
                    s_RotationalPos = Quaternion.Euler(0, 0, 0);
                }
                break;

            case SpawnPoint.GenerateDirection.Left:
                if (s_Turned)
                {
                    s_RotationalPos = Quaternion.Euler(0, 90, 0);
                }
                else
                {
                    s_RotationalPos = Quaternion.Euler(0, 0, 0);
                }
                break;

            case SpawnPoint.GenerateDirection.Up:
                Debug.Log("Up go die!");
                break;

            case SpawnPoint.GenerateDirection.Down:
                Debug.Log("Down go die!");
                break;
        }
    }

    void AddOffset()
    {
        switch(startPoint[s_StartPointIndex].direction)
        {
            case SpawnPoint.GenerateDirection.Right:
                if (s_Turned)
                {
                    s_SpawnPoint.z += -offset;
                }
                else
                {
                    s_SpawnPoint.x += -offset;
                }
                break;

            case SpawnPoint.GenerateDirection.Left:
                if (s_Turned)
                {
                    s_SpawnPoint.z += offset;
                }
                else
                {
                    s_SpawnPoint.x += offset;
                }
                break;

            case SpawnPoint.GenerateDirection.Up:
                Debug.Log("Up go die!");
                break;

            case SpawnPoint.GenerateDirection.Down:
                Debug.Log("Down go die!");
                break;

        }
    }
}
