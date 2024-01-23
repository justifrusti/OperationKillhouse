using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBehaviour : MonoBehaviour
{
    public GameObject[] possibleItems;
    [Range(0.00f, 1.00f)]public float spawnChance = .5f;

    public bool alwaysSpawn;

    public GameObject spawnedTarget;

    void Start()
    {
        if (Random.value < spawnChance)
        {
            /*int index = Random.Range(0, possibleItems.Length);*/

            spawnedTarget = Instantiate(possibleItems[0], transform.position, transform.rotation, transform);
        }else
        {
            /*int index = Random.Range(0, possibleItems.Length);*/

            spawnedTarget = Instantiate(possibleItems[1], transform.position, transform.rotation, transform);
        }
    }
}
