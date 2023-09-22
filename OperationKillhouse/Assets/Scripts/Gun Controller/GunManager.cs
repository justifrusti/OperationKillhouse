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
        [Tooltip("List with attatchment controllers for this gun!")]public List<Attatchement> attatchementControllers;

        [Header("Manager Properties")]
        [Tooltip("Set this to True if you want the player to start with X ammount of Clips")]public bool hasBaseClips = true;
        [ConditionalHide("hasBaseClips")] public int ammountOfBaseClips = 3;
        [Space]
        [Tooltip("Set this to True to enable recoil on the weapon")]public bool useRecoil = true;
        [Space]
        [Tooltip("Set this to True to enable weaponsway on the weapon")] public bool useWeaponSway = true;
        [Space]
        [Tooltip("Set this to True to enable weapon attatchments")]public bool hasAttatchments = true;

        [Header("Animation Properties")]
        [Tooltip("Set this to True if you don't use animation events to fire")] public bool manualAnimTrigger = false;
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

            CheckAttatchments();
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

        public void CheckAttatchments()
        {
            for (int i = 0; i < attatchementControllers.Count; i++)
            {
                switch(attatchementControllers[i].type)
                {
                    case Attatchement.AttatchmentType.ForeGrip:
                        if (gunProperties.weaponAttatchments.foreGrip != gunProperties.weaponAttatchments.GetForeGrip())
                        {
                            gunProperties.weaponAttatchments.SetForeGrip(gunProperties.weaponAttatchments.foreGrip);

                            if (attatchementControllers[i].attatchementID == gunProperties.weaponAttatchments.GetForeGripID())
                            {
                                attatchementControllers[i].CheckForeGrip();
                            }
                            else
                            {
                                attatchementControllers[i].DisableForeGrip();
                            }
                        }
                        break;

                    case Attatchement.AttatchmentType.BackupOptics:
                        if (gunProperties.weaponAttatchments.foreGrip != gunProperties.weaponAttatchments.GetForeGrip())
                        {
                            gunProperties.weaponAttatchments.SetForeGrip(gunProperties.weaponAttatchments.foreGrip);

                            if (attatchementControllers[i].attatchementID == gunProperties.weaponAttatchments.GetForeGripID())
                            {
                                attatchementControllers[i].CheckForeGrip();
                            }
                            else
                            {
                                attatchementControllers[i].DisableForeGrip();
                            }
                        }
                        break;
                }
            }
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

        /// <summary>
        /// Add attatchments to these lists if they are not in here!
        /// </summary>
        [System.Serializable]
        public class Attatchements
        {
            public enum ForeGrips { None, ForeGrip_1, ForeGrip_2, ForeGrip_3 };
            public enum BackupOptics { None, BackupOptics_1, BackupOptics_2, BackupOptics_3 };
            public enum Optics { None, Optics_1, Optics_2, Optics_3 };
            public enum MuzzleDevices { None, MuzzleDevices_1, MuzzleDevices_2, MuzzleDevices_3 };
            public enum Flashlights { None, Flashlights_1, Flashlights_2, Flashlights_3 };
            public enum LaserSights { None, LaserSights_1, LaserSights_2, LaserSights_3 };
            public enum Buttstocks { None, Buttstocks_1, Buttstocks_2, Buttstocks_3 };

            [Tooltip("The number behind the attatchment is the ID of the Attatchment")]public ForeGrips foreGrip;
            [Tooltip("The number behind the attatchment is the ID of the Attatchment")]public Optics optic;
            [Tooltip("The number behind the attatchment is the ID of the Attatchment")]public BackupOptics backupOptics;
            [Tooltip("The number behind the attatchment is the ID of the Attatchment")]public MuzzleDevices muzzleDevices;
            [Tooltip("The number behind the attatchment is the ID of the Attatchment")]public Flashlights flashlights;
            [Tooltip("The number behind the attatchment is the ID of the Attatchment")]public LaserSights laserSights;
            [Tooltip("The number behind the attatchment is the ID of the Attatchment")]public Buttstocks buttstocks;

            [SerializeField]private ForeGrips s_foreGrip;
            private Optics s_optic;
            private BackupOptics s_backupOptics;
            private MuzzleDevices s_muzzleDevices;
            private Flashlights s_flashlights;
            private LaserSights s_laserSights;
            private Buttstocks s_buttstocks;

            //ID Returners
            public int GetForeGripID(){ int attatchmentID = 0; attatchmentID = (int)foreGrip; return attatchmentID; }
            public int GetOpticsID() { int attatchmentID = 0; attatchmentID = (int)optic; return attatchmentID; }
            public int GetBackupOpticsID() { int attatchmentID = 0; attatchmentID = (int)backupOptics; return attatchmentID; }
            public int GetMuzzleDevicesID() { int attatchmentID = 0; attatchmentID = (int)muzzleDevices; return attatchmentID; }
            public int GetFlashlightsID() { int attatchmentID = 0; attatchmentID = (int)flashlights; return attatchmentID; }
            public int GetLaserSightsID() { int attatchmentID = 0; attatchmentID = (int)laserSights; return attatchmentID; }
            public int GetButtStocksID() { int attatchmentID = 0; attatchmentID = (int)buttstocks; return attatchmentID; }

            //Getter and Setters
            public ForeGrips GetForeGrip() { return s_foreGrip; }
            public void SetForeGrip(ForeGrips grip) { s_foreGrip = grip; }
            public Optics GetOptics() { return s_optic; }
            public void SetOptic(Optics grip) { s_optic = grip; }
        }

        [Header("Info")]
        [Tooltip("The Gun name, used for Stat displays")]public string gunName;
        [Tooltip("The Gun description, used for Stat displays")]public string gunDescription;
        [Space]
        [Tooltip("The Gun damage, used for doing damage and Stat displays")]public int gunDamage;
        [Tooltip("The Gun clip size, used to determine how many bullets a gun can hold before reload")]public int clipSize;
        [Space]
        [Tooltip("The ammount of bullets the gun should use per Fire tick")]public int ammoConsumptionPerTick = 1;
        [Space]
        [Tooltip("Enable this if you want the player to keep his magazine with X ammount of bullets on early reload")] public bool hasPartialClips = false;
        [Space]
        [ConditionalHide("useRecoil")][Tooltip("The value that determines the strenght of the recoil")]public float ammountOfRecoil;
        [Space]
        [ConditionalHide("useWeaponSway")][Tooltip("The value that determines the intensity of the weapon sway")] public float weaponSwayIntensity;
        [Space]
        [ConditionalHide("hasAttatchments")]public Attatchements weaponAttatchments;

        [Header("Private Info, DO NOT TOUCH!")]
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
                    for (int i = s_currentClips.Count; i-- > 0;)
                    {
                        ammoToReturn = s_currentClips[i].ammountOfBullets;
                        s_currentClips.Remove(s_currentClips[i]);

                        break;
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
                if(s_ClipAmmo > 0)
                {
                    int newAmmo = clipSize - s_currentAmmo;

                    if(newAmmo <= s_ClipAmmo)
                    {
                        s_ClipAmmo -= newAmmo;
                        ammoToReturn = s_currentAmmo + newAmmo;
                    }else
                    {
                        Debug.Log("Yet to be implemented but my mentally insane brain can't calculate right now!");
                    }
                }else
                {
                    Debug.Log("Out of Ammo!");
                }
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
