using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class Armory : MonoBehaviour {

    public Animation curentGunA;
    public Animation nextGunA;
    public Animation temp;
    public AnimationClip next;
    public AnimationClip back;


    public int mp7Index;
    public int ar15Index;

    public GameObject[] switchButtons;
    public GameObject[] armoryGuns;
    public GameObject[] playerGuns;
    //|Guns|\\
    [Header ("GUNS")]
    public bool mp7;
    public bool ar15;


    public void AnimationPlay ()
    {
        curentGunA.Play ();
        nextGunA.Play ();
        
    }

    public void GunSelect () 
    {
        if(ar15)
        {
            AnimationGunSelct ();

            playerGuns[mp7Index].SetActive(true);
            playerGuns[ar15Index].SetActive(false);
            switchButtons[mp7Index].SetActive(true);
            switchButtons[ar15Index].SetActive(false);
            
            mp7 = true;
            ar15 = false;
        }
        else if(mp7)
        {
            AnimationGunSelct ();

            playerGuns[ar15Index].SetActive(true);
            playerGuns[mp7Index].SetActive(false);
            switchButtons[ar15Index].SetActive(true);
            switchButtons[mp7Index].SetActive(false);
            
            mp7 = false;
            ar15 = true;
        }
    }

    public void AnimationGunSelct () {
        curentGunA.clip = back;
        nextGunA.clip = next;

        AnimationPlay ();
        temp = curentGunA;

        curentGunA = nextGunA;
        nextGunA = temp;
        
    }
}
