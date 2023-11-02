using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBehaviour : MonoBehaviour
{
    public GameObject[] possibleItems;
    [Range(0.00f, 1.00f)]public float spawnChance = .5f;

    void Start()
    {
        if (Random.value < spawnChance)
        {
            int index = Random.Range(0, possibleItems.Length);

            Instantiate(possibleItems[index], transform.position, possibleItems[index].transform.rotation, transform);
        }
    }
}
