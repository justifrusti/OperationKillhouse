using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public Animator nPC;
    public Animator aR;
    public Animator pistol;
     
    public float timer;

    public AudioSource piew;
    public AudioSource smalPiew;

    public bool test;
    void Start() {

        timer = 60f;
        
    }

    void Update() {

       // timer -= Time.deltaTime;

        if (timer <= 0) {
            nPC.SetTrigger ("GO");
            aR.SetTrigger ("GO");
            pistol.SetTrigger ("GO");
        }

        if(test == true) {
            nPC.SetTrigger ("GO");
            aR.SetTrigger ("GO");
            pistol.SetTrigger ("GO");
            test = false;
        }

        if ( Input.GetKey (KeyCode.N)) {
            nPC.SetTrigger ("GO");
            aR.SetTrigger ("GO");
            pistol.SetTrigger ("GO");
        }
    }

    public void AinimationDone () {
       // timer = 60f;
    }

    public void Piew () {
        piew.Play ();
    }

    public void PiewSmall () {
        smalPiew.Play ();
    }

    public void Casing () {

    }

    public void CasingSmall () {

    }

}
