using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTestScript : MonoBehaviour {

    public Animator gun;
    public Animator arm;

    void Start() {
        
    }

    
    void Update() {

        if (Input.GetButton ("Fire1")) {
            gun.SetBool ("Fire" , true);
            arm.SetBool ("Fire" , true);
        } else {
            gun.SetBool ("Fire" , false);
            arm.SetBool ("Fire" , false);
        }

        if (Input.GetButton ("Fire2")) {
            gun.SetBool ("Aim" , true);
            arm.SetBool ("Aim" , true);
        } else {
            gun.SetBool ("Aim" , false);
            arm.SetBool ("Aim" , false);
        }

        if (Input.GetKeyDown (KeyCode.E)) {
            gun.SetBool ("Empty" , true);
            arm.SetBool ("Empty" , true);
        } else {
            gun.SetBool ("Empty" , false);
            arm.SetBool ("Empty" , false);
        }


        if (Input.GetKeyDown (KeyCode.R)) {
            gun.SetTrigger ("Reload");
            arm.SetTrigger ("Reload");
        }
        if (Input.GetKeyDown (KeyCode.C)) {
            gun.SetTrigger ("Check");
            arm.SetTrigger ("Check");
        }
        if (Input.GetKeyDown (KeyCode.H)) {
            gun.SetTrigger ("Holster");
            arm.SetTrigger ("Holster");
        }
    }
}
