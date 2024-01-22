using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawnPlayer : MonoBehaviour
{
    Transform spawnPoint;
    GameObject player;
    void Start()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void ReSpawn()
    {
        player.transform.position = spawnPoint.position;
    }
}
