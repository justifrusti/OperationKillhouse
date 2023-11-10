using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class Armory : MonoBehaviour {

    public Animation curentGunA;
    public Animation nextGunA;

    public int mp7Index;
    public int ar15Index;

    public GameObject[] switchButtons;
    public GameObject[] armoryGuns;
    public GameObject[] playerGuns;
    //|Guns|\\
    [HideInInspector]
    public bool mp7, ar15;


    public void AnimationPlay ()
    {
        curentGunA.Play ();
        nextGunA.Play ();
    }

    public void GunSelect () 
    {
        if(mp7 && !armoryGuns[mp7Index].activeInHierarchy)
        {
            armoryGuns[mp7Index].SetActive(true);
            armoryGuns[ar15Index].SetActive(false);
            switchButtons[mp7Index].SetActive(true);
            switchButtons[ar15Index].SetActive(false);
        }
        else if(ar15 && !armoryGuns[ar15Index].activeInHierarchy)
        {
            armoryGuns[ar15Index].SetActive(true);
            armoryGuns[mp7Index].SetActive(false);
            switchButtons[ar15Index].SetActive(true);
            switchButtons[mp7Index].SetActive(false);
        }
    }
}
