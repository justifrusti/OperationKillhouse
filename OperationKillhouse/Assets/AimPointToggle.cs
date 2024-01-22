using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimPointToggle : MonoBehaviour
{
    public GameObject aimPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(aimPoint.activeSelf == false)
        {
            aimPoint.SetActive(true);
        }
        else
        {
            aimPoint.SetActive(false);
        }
    }
}
