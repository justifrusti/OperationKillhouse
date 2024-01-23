using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawnPlayer : MonoBehaviour
{
    Transform spawnPoint;
    GameObject player;
    ScoreManager scoreManager;
    void Start()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        player = GameObject.FindGameObjectWithTag("Player");
        scoreManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ScoreManager>();
    }

    public void ReSpawn()
    {
        player.transform.position = spawnPoint.position;
        scoreManager.ResetTimer();
    }
}
