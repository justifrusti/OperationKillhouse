using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    RaycastHit hit;
    public GameObject light;
    void Update()
    {
        light.transform.LookAt(GetComponentInParent<playerController>().cam);
        GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
        if(Physics.Raycast(this.transform.position, transform.forward, out hit, Mathf.Infinity, -5, QueryTriggerInteraction.Ignore))
        {
            GetComponent<LineRenderer>().SetPosition(1, hit.point);
            light.transform.position = hit.point;
        }
    }
}
