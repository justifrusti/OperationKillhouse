using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimPointToggle : MonoBehaviour
{
    public GameObject aimPoint;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            aimPoint.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            aimPoint.SetActive(false);
        }
    }

}
