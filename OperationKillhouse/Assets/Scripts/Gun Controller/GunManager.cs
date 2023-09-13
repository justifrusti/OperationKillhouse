using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gun
{
    [Serializable]
    public class GunManager : MonoBehaviour
    {
        public GunProperties gunProperties = new GunProperties();

        [System.Serializable]
        public class Clip
        {
            public int ammountOfBullets;

            public bool isEmpty;
        }

        [Header("Manager Properties")]
        [Tooltip("Set this to True if you want the player to start with X ammount of Clips")]public bool hasBaseClips = true;
        [ConditionalHide("hasBaseClips")] public int ammountOfBaseClips = 3;
        [Space]
        [Tooltip("Enable this if you want the player to keep his magazine with X ammount of bullets on early reload")]public bool hasPartialClips = false;
        [ConditionalHide("hasPartialClips")] public List<Clip> currentClips;

        [Header("Animation Properties")]
        public bool manualAnimTrigger = false;
        [Space]
        [ConditionalHide("manualAnimTrigger")]public Animator anim;
        [ConditionalHide("manualAnimTrigger")]public string animTriggerName;

        [Header("Debug")]
        public bool testFireFunc = false;

        private void Start()
        {
            if(hasBaseClips)
            {
                gunProperties.SetClipAmmo(gunProperties.clipSize * ammountOfBaseClips);
            }
            else
            {
                gunProperties.SetClipAmmo(gunProperties.clipSize);
            }
        }

        private void Update()
        {
            if(testFireFunc)
            {
                testFireFunc = false;

                Fire();
            }
        }

        public void Fire()
        {
            if(gunProperties.GetCurrentAmmo() > 0)
            {
                gunProperties.SetUsedClipAmmo(gunProperties.ammoConsumptionPerTick);
            }else
            {
                gunProperties.SetCurrentAmmo();
            }
        }
    }

    [System.Serializable]
    public class GunProperties
    {
        [Header("Info")]
        [Tooltip("The Gun name, used for Stat displays")]public string gunName;
        [Tooltip("The Gun description, used for Stat displays")]public string gunDescription;

        [Tooltip("The Gun damage, used for doing damage and Stat displays")]public int gunDamage;
        [Tooltip("The Gun clip size, used to determine how many bullets a gun can hold before reload")]public int clipSize;

        [Tooltip("The ammount of bullets the gun should use per Fire tick")]public int ammoConsumptionPerTick = 1;

        //s_Info (DO NOT CHANGE IN CODE!)
        [SerializeField][Tooltip("The current gun ammo, DO NOT CHANGE THIS NUMBER DIRECTLY IN CODE!")]private int s_currentAmmo;
        [SerializeField][Tooltip("The ammount of ammo the gun has left for reloading, DO NOT CHANGE THIS NUMBER DIRECTLY IN CODE!")]private int s_ClipAmmo;
      

        /// <summary>
        /// Returns the current ammo that the gun has!
        /// </summary>
        public int GetCurrentAmmo()
        {
            return s_currentAmmo;
        }

        /// <summary>
        /// Sets the current ammo the gun has to shoot (for reloading)!
        /// </summary>
        public int SetCurrentAmmo()
        {
            int ammoToReturn = 0;

            if (s_ClipAmmo >= clipSize)
            {
                s_ClipAmmo -= clipSize;

                ammoToReturn = clipSize;
            }
            else if (s_ClipAmmo < clipSize)
            {
                int newAmmo = clipSize - s_ClipAmmo;

                ammoToReturn = clipSize - newAmmo;
            }

            return s_currentAmmo = ammoToReturn;
        }

        /// <summary>
        /// Returns the ammo that the gun has left for reloading!
        /// </summary>
        public int GetClipAmmo()
        {
            return s_ClipAmmo;
        }

        /// <summary>
        /// Sets the new ammo ammount for when ammo is picked up!
        /// </summary>
        public int SetClipAmmo(int ammoToAdd)
        {
            return s_ClipAmmo = s_ClipAmmo + ammoToAdd;
        }

        /// <summary>
        /// Depletes ammo upon shooting!
        /// </summary>
        public int SetUsedClipAmmo(int ammoToSubtract)
        {
            return s_currentAmmo = s_currentAmmo - ammoToSubtract;
        }
    }
}
