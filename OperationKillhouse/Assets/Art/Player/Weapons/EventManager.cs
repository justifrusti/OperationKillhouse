using Gun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public Ammocounter ammoCounter;
    public GunManager gunManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MagBullet () {
        ammoCounter.Mag();
    }

    public void Casing () {
        print("yeet");
    }

    public void Fire () {
        gunManager.Fire();
    }

    public void Reload () {
        gunManager.Reload();
    }
}
