using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoomInfo : MonoBehaviour
{
    public GameObject collisionCheckOBJ;
    ScoreManager scoreManager;


    private void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<ScoreManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            scoreManager.stopTimer();
            scoreManager.TurnOnText();
        }
    }
}
