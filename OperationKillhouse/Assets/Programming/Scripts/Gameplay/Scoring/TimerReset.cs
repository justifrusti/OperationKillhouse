using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RoomInteriorManager;

public class TimerReset : MonoBehaviour
{
    public GameObject respawnUI;
    public ScoreManager scoreManager;

    public GameObject player;
    public GameObject respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            respawnUI.SetActive(true);
            scoreManager.stopTimer();
        }
    }
    public void respawn()
    {
        player.transform.position = respawnPoint.transform.position;
    }
}
