using Gun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlockFirstPick : MonoBehaviour {

    public Animator arm;
    public Animator glock;
    public GunManager gunManager;
    public bool pickt;


    void Start() {
        


    }

    
    void Update() {
        
     //  if(gunManager.gunProperties.GetCurrentAmmo() <= 1) {
     //       glock.SetBool ("LastBullet" , true);
     //       arm.SetBool ("LastBullet" , true);
     //   } else {
     //      glock.SetBool ("LastBullet" , false);
     //       arm.SetBool ("LastBullet" , false);
     //   }


        if(pickt == true) {
            glock.SetBool ("FirstDraw" , false);
            arm.SetBool ("FirstDraw" , false);
        }
        else {
            glock.SetBool ("FirstDraw" , true);
            arm.SetBool ("FirstDraw" , true);
        }
    }
}
