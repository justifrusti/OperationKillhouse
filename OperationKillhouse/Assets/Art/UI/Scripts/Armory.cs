using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class Armory : MonoBehaviour {

    public Animation curentGunA;
    public Animation nextGunA;

    public GameObject curentGun;
    public GameObject nextGun;

    //|Guns|\\
    [HideInInspector]
    public bool mp7, ar15;
    

    public void AnimationPlay () {
        curentGunA.Play ();
        nextGunA.Play ();
    } 

    public void GunSelect () {

    }
}
