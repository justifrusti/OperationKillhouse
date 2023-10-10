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

        if (Input.GetKey (KeyCode.E)) {
            gun.SetBool ("Empty" , true);
            arm.SetBool ("Empty" , true);
        } else {
            gun.SetBool ("Empty" , false);
            arm.SetBool ("Empty" , false);
        }


        if (Input.GetKey (KeyCode.R)) {
            gun.SetTrigger ("Reload");
            arm.SetTrigger ("Reload");
        }
        if (Input.GetKey (KeyCode.C)) {
            gun.SetTrigger ("Check");
            arm.SetTrigger ("Check");
        }
        if (Input.GetKey (KeyCode.H)) {
            gun.SetTrigger ("Holster");
            arm.SetTrigger ("Holster");
        }
    }
}
