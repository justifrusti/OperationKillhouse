using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammocounter : MonoBehaviour {

    public Image actifMag;
    public Sprite[] mags;
    int ammo = 0;
    public Animation bullet;

    void Start() {
        

    }

    
    void Update() {


    }
    
    public void AnimPlay() {
        bullet.Play();
    }

    public void Mag () {
        if(ammo < mags.Length - 1)
        {
            ammo++;
            actifMag.sprite = mags[ammo];
        }else
        {
            Debug.Log("Implement Case Removal");
        }
    }
}
