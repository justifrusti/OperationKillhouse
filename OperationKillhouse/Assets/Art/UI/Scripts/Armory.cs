using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class Armory : MonoBehaviour {

    public Animation curentGunA;
    public Animation nextGunA;

    public GameObject[] guns;
    //|Guns|\\
    [HideInInspector]
    public bool mp7, ar15;

    GameObject currentGun;
    int index = 0;


    public void AnimationPlay ()
    {
        curentGunA.Play ();
        nextGunA.Play ();
    }

    public void GunSelect () 
    {
        if(index <  guns.Length)
        {
            currentGun = guns[index + 1];
        }else
        {
            index = 0;
            currentGun = guns[index];
        }

        index++;
    }
}
