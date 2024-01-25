using Gun;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    //public Ammocounter ammoCounter;
    public GunManager gunManager;
    public playerController playerController;
    public GlockFirstPick GlockFirstPick;
    public GameObject firePoint;

    [Header("SOUND")]
    public AudioClip fireS;
    public AudioClip fire;
    public AudioClip sound1, sound2, sound3, sound4, sound5, sound6, soundMag1, soundMag2;
    public AudioSource audioSource;
    public AudioSource audioSourceMag;

    [Header("EFX")]
    public ParticleSystem eFXFire;
    public ParticleSystem eFXFireS;

    public bool isSuppresed;
    
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void MagBullet () {
        //ammoCounter.Mag();
    }

    public void Casing () {
        
    }

    public void MagDrop()
    {
        Instantiate(gunManager.gunProperties.emptyMag, gunManager.gunProperties.enmptyMagSpawnPoint);
    }

    public void Fire () {
        if (isSuppresed == true) {
            firePoint.GetComponent<AudioSource>().clip = fireS;
        }
        else {
            eFXFire.Play();
            firePoint.GetComponent<AudioSource> ().clip = fire;
        }
        gunManager.Fire();
        firePoint.GetComponent<AudioSource> ().Play ();
    }

    public void Reload () {
        gunManager.Reload();
    }

    public void Holster () {
        playerController.WeaponSwapping ();
    }

    public void FirstPick () {
        GlockFirstPick.pickt = true;
    }

    public void Sound1 () {
        audioSource.clip = sound1;
        audioSource.Play();
    }
    public void Sound2 () {
        audioSource.clip = sound2;
        audioSource.Play();
    }
    public void Sound3 () {
        audioSource.clip = sound3;
        audioSource.Play();
    }
    public void Sound4 () {
        audioSource.clip = sound4;
        audioSource.Play();
    }
    public void Sound5 () {
        audioSource.clip = sound5;
        audioSource.Play();
    }
    public void Sound6 () {
        audioSource.clip = sound6;
        audioSource.Play();
    }

    public void SoundMag1 () {
        audioSourceMag.clip = soundMag1;
        audioSourceMag.Play();
    }
    public void SoundMag2 () {
        audioSourceMag.clip = soundMag2;
        audioSourceMag.Play();
    }
    public void WeaponActif () {
        playerController.animEvent.gunManager = GameObject.FindGameObjectWithTag ("Weapon").GetComponent<GunManager> ();
        playerController.animEvent.armAnimator = GameObject.FindGameObjectWithTag ("Arm").GetComponent<Animator> ();
        playerController.animEvent.gunAnimator = GameObject.FindGameObjectWithTag ("Gun").GetComponent<Animator> ();
    }

    public void FireSelect()
    {
        if (playerController.animEvent.gunAnimator.GetBool("FullAuto"))
        {
            playerController.animEvent.gunAnimator.SetBool("Semi", true);
            playerController.animEvent.gunAnimator.SetBool("FullAuto", false);
        }
        else if (playerController.animEvent.gunAnimator.GetBool("Semi"))
        {
            playerController.animEvent.gunAnimator.SetBool("FullAuto", true);
            playerController.animEvent.gunAnimator.SetBool("Semi", false);
        }
    }

    public void Click()
    {
        if(gunManager.gunProperties.GetCurrentAmmo() == 0)
        {
            audioSource.clip = sound6;
            audioSource.Play();
        }
    }
}
