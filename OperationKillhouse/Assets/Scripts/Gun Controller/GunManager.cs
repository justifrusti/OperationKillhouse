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

        [Header("Manager Properties")]
        [Tooltip("Set this to True if you want the player to start with X ammount of Clips")]public bool hasBaseClips = true;
        [ConditionalHide("hasBaseClips")] public int ammountOfBaseClips = 3;

        [Header("Animation Properties")]
        public bool manualAnimTrigger = false;
        [Space]
        [ConditionalHide("manualAnimTrigger")]public Animator anim;
        [ConditionalHide("manualAnimTrigger")]public string animTriggerName;

        [Header("Debug")]
        public bool testFireFunc = false;
        public bool testEarlyReload = false;

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

            if(testEarlyReload)
            {
                testEarlyReload = false;

                SwapClip();
            }
        }

        /// <summary>
        /// The fire function of the gun, call this function to shoot!
        /// </summary>
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

        /// <summary>
        /// The reload/clip swap function, call this to stock a partially emptied clip or reload the gun!
        /// </summary>
        public void SwapClip()
        {
            gunProperties.SetCurrentAmmo();
        }
    }

    [System.Serializable]
    public class GunProperties
    {
        [System.Serializable]
        public class Clip
        {
            public int ammountOfBullets;

            public bool isEmpty;
        }

        [Header("Info")]
        [Tooltip("The Gun name, used for Stat displays")]public string gunName;
        [Tooltip("The Gun description, used for Stat displays")]public string gunDescription;

        [Tooltip("The Gun damage, used for doing damage and Stat displays")]public int gunDamage;
        [Tooltip("The Gun clip size, used to determine how many bullets a gun can hold before reload")]public int clipSize;

        [Tooltip("The ammount of bullets the gun should use per Fire tick")]public int ammoConsumptionPerTick = 1;

        [Tooltip("Enable this if you want the player to keep his magazine with X ammount of bullets on early reload")] public bool hasPartialClips = false;

        //s_Info (DO NOT CHANGE IN CODE!)
        [SerializeField][Tooltip("The current gun ammo, DO NOT CHANGE THIS NUMBER DIRECTLY IN CODE!")]private int s_currentAmmo;
        [SerializeField][Tooltip("The ammount of ammo the gun has left for reloading, DO NOT CHANGE THIS NUMBER DIRECTLY IN CODE!")]private int s_ClipAmmo;
        [SerializeField][Tooltip("The ammount of partial clips the player has (clips that are not full), DO NOT CHANGE THIS LIST DIRECTLY IN CODE!")]private List<Clip> s_currentClips;

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

            if(s_currentAmmo <= 0)
            {
                if(s_ClipAmmo > 0)
                {
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
                }else if(s_ClipAmmo <= 0 && s_currentClips.Count > 0 && hasPartialClips)
                {
                    for (int i = s_currentClips.Count; i > 0; i--)
                    {
                        ammoToReturn = s_currentClips[i].ammountOfBullets;
                        s_currentClips.Remove(s_currentClips[i]);
                    }
                }else
                {
                    Debug.Log("Out of bullets!");
                }
            }else if(hasPartialClips)
            {
                SwapClip(s_currentClips);
            }else
            {
                Debug.Log("No partial clips enabled!");
            }

            return s_currentAmmo = ammoToReturn;
        }

        /// <summary>
        /// Swap the current clip you are using with a full one, keeps the clip with X ammount of bullets! NOTE: hasPartialClips needs to be enabled for this to do anything.
        /// </summary>
        public void SwapClip(List<Clip> clips)
        {
            int currentBullets = GetCurrentAmmo();
            s_currentAmmo = 0;

            Clip newClip = new Clip();

            clips.Add(newClip);
            
            if(clips.Count <= 1)
            {
                clips[0].ammountOfBullets = currentBullets;
            }
            else
            {
                clips[clips.Count - 1].ammountOfBullets = currentBullets;
            }
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
