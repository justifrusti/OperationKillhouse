using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRandomizer : MonoBehaviour
{
    public GameObject[] arrowType;
    
    private void Start()
    {
        ArrowSelect(arrowType);
    }

    void ArrowSelect(GameObject[] arrowArray)
    {
        int i = Random.Range(0, arrowArray.Length);
        arrowArray[i].SetActive(true);
    }
}