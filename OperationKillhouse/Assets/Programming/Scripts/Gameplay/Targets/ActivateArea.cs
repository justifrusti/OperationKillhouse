using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateArea : MonoBehaviour
{
    public PointBehaviour[] spawnPoints;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            acivateTargets();
        }
    }

    void acivateTargets()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i].spawnedTarget != null)
            {
                spawnPoints[i].spawnedTarget.GetComponentInChildren<EnemyStats>().PopUp();
            }
        }
    }
}
