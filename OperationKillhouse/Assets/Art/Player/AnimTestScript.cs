using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimTestScript : MonoBehaviour {
    [Header ("GUNS")]
    public GameObject gun1;
    public GameObject gun2;
    public GameObject gun3;

    [Header ("ANIMCOMP")]
    public Animator gunA;
    public Animator armA;

    void Start() {
        
    }

    
    void Update() {

        //weaponswap\\
        if (Input.GetKey (KeyCode.Alpha1)) {
            gun1.SetActive (true);
            gun2.SetActive (false);
            gun3.SetActive (false);
            
            gunA = GameObject.FindGameObjectWithTag ("Gun").GetComponent<Animator> ();
            armA = GameObject.FindGameObjectWithTag ("Arm").GetComponent<Animator> ();
        }
        if (Input.GetKey (KeyCode.Alpha2)) {
            gun1.SetActive (false);
            gun2.SetActive (true);
            gun3.SetActive (false);
            
            gunA = GameObject.FindGameObjectWithTag ("Gun").GetComponent<Animator> ();
            armA = GameObject.FindGameObjectWithTag ("Arm").GetComponent<Animator> ();
        }
        if (Input.GetKey (KeyCode.Alpha3)) {
            gun1.SetActive (false);
            gun2.SetActive (false);
            gun3.SetActive (true);
            
            gunA = GameObject.FindGameObjectWithTag ("Gun").GetComponent<Animator> ();
            armA = GameObject.FindGameObjectWithTag ("Arm").GetComponent<Animator> ();
        }

        //otherstuff\\
        if (Input.GetButton ("Fire1")) {
            gunA.SetBool ("Fire" , true);
            armA.SetBool ("Fire" , true);
        } else {
            gunA.SetBool ("Fire" , false);
            armA.SetBool ("Fire" , false);
        }

        if (Input.GetButton ("Fire2")) {
            gunA.SetBool ("Aim" , true);
            armA.SetBool ("Aim" , true);
        } else {
            gunA.SetBool ("Aim" , false);
            armA.SetBool ("Aim" , false);
        }

        if (Input.GetKey (KeyCode.E)) {
            gunA.SetBool ("Empty" , true);
            armA.SetBool ("Empty" , true);
        } else {
            gunA.SetBool ("Empty" , false);
            armA.SetBool ("Empty" , false);
        }

        if (Input.GetKey (KeyCode.L)) {
            gunA.SetBool ("LastBullet" , true);
            armA.SetBool ("LastBullet" , true);
        } else {
            gunA.SetBool ("LastBullet" , false);
            armA.SetBool ("LastBullet" , false);
        }
    

        if (Input.GetKeyDown (KeyCode.R)) {
            gunA.SetTrigger ("Reload");
            armA.SetTrigger ("Reload");
        }
        if (Input.GetKeyDown (KeyCode.C)) {
            gunA.SetTrigger ("Check");
            armA.SetTrigger ("Check");
        }
        if (Input.GetKeyDown (KeyCode.H)) {
            gunA.SetTrigger ("Holster");
            armA.SetTrigger ("Holster");
        }
    }
}
