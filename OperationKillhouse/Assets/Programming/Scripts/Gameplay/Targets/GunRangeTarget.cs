using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRangeTarget : MonoBehaviour
{
    List<GameObject> bulletHoles;
    public Animator animator;
    public GameObject bulletHole;

    void Start()
    {
        
    }

    void Update()
    {
        var targets = GameObject.FindGameObjectsWithTag("Rage Target");

        for (int i = 0; i < targets.Length; i++)
        {
            if (!bulletHoles.Contains(targets[i]))
            {
                bulletHoles.Add(targets[i]);
            }
        }
    }

    void RemoveBulletHoles()
    {
        for (int i = 0; i < bulletHoles.Count; i++)
        {
            Destroy(bulletHoles[i]);
            bulletHoles.RemoveAt(i);
        }
    }
}
