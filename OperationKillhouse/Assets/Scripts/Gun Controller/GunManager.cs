using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

#pragma warning disable IDE0090
namespace Gun
{
    [Serializable]
    public class GunManager : MonoBehaviour
    {
        public GunProperties gunProperties = new GunProperties();
        [Tooltip("List with attatchment controllers for this gun!")]public List<Attatchement> attatchementControllers;

        public bool aiming;

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
        public bool testEarlyReload = false;
        public bool fire = false;
        public bool shootingDebugRayActive = false;

        //Privates
        Vector3 s_RotationRecoil;
        Vector3 s_PositionRecoil;
        Vector3 s_Rot;

        Quaternion localRotation;

        playerController s_Controller;

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

            if(GameObject.FindGameObjectWithTag("Player") != null && GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>() != null)
            {
                s_Controller = GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>();
            }else
            {
                Debug.LogError("Error: Player or Player Controller not present. Please make sure your player has the 'Player' tag and has a PlayerController"); 
            }

            localRotation = transform.localRotation;
        }

        private void Update()
        {
            if(testEarlyReload)
            {
                testEarlyReload = false;

                SwapClip();
            }

            if(fire)
            {
                fire = false;
                Fire();
            }

            if(useRecoil)
            {
                RecoilUpdate();
            }

            if (useWeaponSway)
            {
                WeaponSwayUpdate();
            }

            CheckAttatchments();
        }

        /// <summary>
        /// The fire function of the gun, call this function to shoot!
        /// </summary>
        public void Fire()
        {
            if (gunProperties.GetCurrentAmmo() > 0)
            {
                if (aiming)
                {
                    s_RotationRecoil += new Vector3(-gunProperties.recoilRotationAim.x, gunProperties.recoilRotationAim.y, gunProperties.recoilRotationAim.z);
                    s_PositionRecoil += new Vector3(gunProperties.recoilAmountAim.x, gunProperties.recoilAmountAim.y, gunProperties.recoilAmountAim.z);
                }
                else
                {
                    s_RotationRecoil += new Vector3(-gunProperties.recoilRot.x, Random.Range(-gunProperties.recoilRot.y, gunProperties.recoilRot.y), Random.Range(-gunProperties.recoilRot.z, gunProperties.recoilRot.z));
                    s_RotationRecoil += new Vector3(Random.Range(-gunProperties.ammountOfRecoil.x, gunProperties.ammountOfRecoil.x), Random.Range(-gunProperties.ammountOfRecoil.y, gunProperties.ammountOfRecoil.y), gunProperties.ammountOfRecoil.z);
                }

                switch (gunProperties.shootingType)
                {
                    case GunProperties.ShootingType.Raycast:
                        if (shootingDebugRayActive)
                        {
                            Debug.DrawRay(GetRay().origin, GetRay().direction * 500f, Color.red);
                        }

                        RaycastHit hit;

                        if (Physics.Raycast(GetRay(), out hit, 500f))
                        {
                            if (hit.collider.CompareTag("Red Target"))
                            {
                                hit.collider.GetComponent<EnemyStats>().Damage(gunProperties.gunDamage);
                            }
                            else if (hit.collider.CompareTag("Blue Target"))
                            {
                                ScoreManager.instance.AddPenalty(10);
                                hit.collider.GetComponent<EnemyStats>().Damage(gunProperties.gunDamage);
                            }
                        }
                        break;

                    case GunProperties.ShootingType.Physics:
                        Debug.Log("Physics shooting system does not exsist (yet)");
                        break;
                }

                gunProperties.SetUsedClipAmmo(gunProperties.ammoConsumptionPerTick);
            }
        }

        public void Reload()
        {
            int tempReloadAmmo = gunProperties.GetClipAmmo();

            SwapClip();

            if(gunProperties.GetCurrentAmmo() <= 0)
            {
                gunProperties.SetCurrentClipAmmo();

                if(tempReloadAmmo >= gunProperties.GetClipAmmo())
                {
                    gunProperties.SubtractClipAmmo(gunProperties.clipSize);
                }
            }
        }

        /// <summary>
        /// The reload/clip swap function, call this to stock a partially emptied clip or reload the gun!
        /// </summary>
        private void SwapClip()
        {
            gunProperties.SetCurrentAmmo();
        }

        private void CheckAttatchments()
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
                                attatchementControllers[i].CheckAttatcment();
                            }
                            else
                            {
                                attatchementControllers[i].DisableAttatchment();
                            }
                        }
                        break;

                    case Attatchement.AttatchmentType.Optics:
                        if (gunProperties.weaponAttatchments.optic != gunProperties.weaponAttatchments.GetOptics())
                        {
                            gunProperties.weaponAttatchments.SetOptic(gunProperties.weaponAttatchments.optic);

                            if (attatchementControllers[i].attatchementID == gunProperties.weaponAttatchments.GetOpticsID())
                            {
                                attatchementControllers[i].CheckAttatcment();
                            }
                            else
                            {
                                attatchementControllers[i].DisableAttatchment();
                            }
                        }
                        break;

                    case Attatchement.AttatchmentType.BackupOptics:
                        if (gunProperties.weaponAttatchments.backupOptics != gunProperties.weaponAttatchments.GetBackupOptics())
                        {
                            gunProperties.weaponAttatchments.SetBackupOptics(gunProperties.weaponAttatchments.backupOptics);

                            if (attatchementControllers[i].attatchementID == gunProperties.weaponAttatchments.GetBackupOpticsID())
                            {
                                attatchementControllers[i].CheckAttatcment();
                            }
                            else
                            {
                                attatchementControllers[i].DisableAttatchment();
                            }
                        }
                        break;

                    case Attatchement.AttatchmentType.MuzzleDevice:
                        if (gunProperties.weaponAttatchments.muzzleDevices != gunProperties.weaponAttatchments.GetMuzzleDevices())
                        {
                            gunProperties.weaponAttatchments.SetMuzzleDevices(gunProperties.weaponAttatchments.muzzleDevices);

                            if (attatchementControllers[i].attatchementID == gunProperties.weaponAttatchments.GetMuzzleDevicesID())
                            {
                                attatchementControllers[i].CheckAttatcment();
                            }
                            else
                            {
                                attatchementControllers[i].DisableAttatchment();
                            }
                        }
                        break;

                    case Attatchement.AttatchmentType.Flashlight:
                        if (gunProperties.weaponAttatchments.flashlights != gunProperties.weaponAttatchments.GetFlashlights())
                        {
                            gunProperties.weaponAttatchments.SetFlashlights(gunProperties.weaponAttatchments.flashlights);

                            if (attatchementControllers[i].attatchementID == gunProperties.weaponAttatchments.GetFlashlightsID())
                            {
                                attatchementControllers[i].CheckAttatcment();
                            }
                            else
                            {
                                attatchementControllers[i].DisableAttatchment();
                            }
                        }
                        break;

                    case Attatchement.AttatchmentType.Lasersight:
                        if (gunProperties.weaponAttatchments.laserSights != gunProperties.weaponAttatchments.GetLaserSights())
                        {
                            gunProperties.weaponAttatchments.SetLaserSights(gunProperties.weaponAttatchments.laserSights);

                            if (attatchementControllers[i].attatchementID == gunProperties.weaponAttatchments.GetLaserSightsID())
                            {
                                attatchementControllers[i].CheckAttatcment();
                            }
                            else
                            {
                                attatchementControllers[i].DisableAttatchment();
                            }
                        }
                        break;

                    case Attatchement.AttatchmentType.Buttstock:
                        if (gunProperties.weaponAttatchments.buttstocks != gunProperties.weaponAttatchments.GetButtstocks())
                        {
                            gunProperties.weaponAttatchments.SetButtstocks(gunProperties.weaponAttatchments.buttstocks);

                            if (attatchementControllers[i].attatchementID == gunProperties.weaponAttatchments.GetButtStocksID())
                            {
                                attatchementControllers[i].CheckAttatcment();
                            }
                            else
                            {
                                attatchementControllers[i].DisableAttatchment();
                            }
                        }
                        break;
                }
            }
        }

        private void RecoilUpdate()
        {
            s_RotationRecoil = Vector3.Lerp(s_RotationRecoil, Vector3.zero, gunProperties.rotReturnSpeed * Time.deltaTime);
            s_PositionRecoil = Vector3.Lerp(s_PositionRecoil, Vector3.zero, gunProperties.posReturnSpeed * Time.deltaTime);

            gunProperties.recoilPos.localPosition = Vector3.Slerp(gunProperties.recoilPos.localPosition, s_PositionRecoil, gunProperties.positionRecoilSpeed * Time.deltaTime);
            s_Rot = Vector3.Slerp(s_Rot, s_RotationRecoil, gunProperties.rotationRecoilSpeed * Time.deltaTime);
            gunProperties.recoilPos.localRotation = Quaternion.Euler(s_Rot);


            if (Input.GetButtonDown("Fire2"))
            {
                aiming = true;
            }else if(Input.GetButtonUp("Fire2"))
            {
                aiming = false;
            }

        }

        private void WeaponSwayUpdate()
        {
            float z = (Input.GetAxis("Mouse Y")) * gunProperties.drag;
            float y = -(Input.GetAxis("Mouse X")) * gunProperties.drag;

            if (gunProperties.drag >= 0) //weapon lags behind camera
            {
                y = (y > gunProperties.dragThreshold) ? gunProperties.dragThreshold : y;
                y = (y < -gunProperties.dragThreshold) ? -gunProperties.dragThreshold : y;

                z = (z > gunProperties.dragThreshold) ? gunProperties.dragThreshold : z;
                z = (z < -gunProperties.dragThreshold) ? -gunProperties.dragThreshold : z;
            }
            else //camera lags behind weapon
            {
                y = (y < gunProperties.dragThreshold) ? gunProperties.dragThreshold : y;
                y = (y > -gunProperties.dragThreshold) ? -gunProperties.dragThreshold : y;

                z = (z < gunProperties.dragThreshold) ? gunProperties.dragThreshold : z;
                z = (z > -gunProperties.dragThreshold) ? -gunProperties.dragThreshold : z;
            }

            Quaternion newRotation = Quaternion.Euler(localRotation.x, localRotation.y + y, localRotation.z + z);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, newRotation, (Time.deltaTime * gunProperties.smooth));
        }

        Ray GetRay()
        {
            Vector3 rayOrigin = new Vector3(.5f, .5f, 0f);

            return Camera.main.ViewportPointToRay(rayOrigin);
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

            private ForeGrips s_foreGrip;
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
            public void SetOptic(Optics optic) { s_optic = optic; }
            public BackupOptics GetBackupOptics() { return s_backupOptics; }
            public void SetBackupOptics(BackupOptics bOptics) { s_backupOptics = bOptics; }
            public MuzzleDevices GetMuzzleDevices() {  return s_muzzleDevices; }
            public void SetMuzzleDevices(MuzzleDevices muzzle) { s_muzzleDevices = muzzle; }
            public Flashlights GetFlashlights() { return s_flashlights; }
            public void SetFlashlights(Flashlights flashlight) { s_flashlights = flashlight; }
            public LaserSights GetLaserSights() { return s_laserSights; }
            public void SetLaserSights(LaserSights laserSight) { s_laserSights = laserSight; }
            public Buttstocks GetButtstocks() { return s_buttstocks; }
            public void SetButtstocks(Buttstocks buttstock) { s_buttstocks = buttstock; }
        }

        public enum ShootingType { Raycast, Physics };
        [Header("Info")]
        [Tooltip("The way the bullets 'Fire' from the gun")]public ShootingType shootingType;
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
        [ConditionalHide("useRecoil")][Tooltip("The transform point where the recoil gets applied from")]public Transform recoilPos;
        [ConditionalHide("useRecoil")]public Vector3 recoilRot = new Vector3(10, 5, 7);
        [ConditionalHide("useRecoil")][Tooltip("The value that determines the strenght of the recoil")]public Vector3 ammountOfRecoil;
        [Space]
        [ConditionalHide("useRecoil")]public float positionRecoilSpeed = 8f;
        [ConditionalHide("useRecoil")]public float rotationRecoilSpeed = 8f;
        [Space]
        [ConditionalHide("useRecoil")]public float posReturnSpeed = 18f;
        [ConditionalHide("useRecoil")]public float rotReturnSpeed = 38f;
        [Space]
        [ConditionalHide("useRecoil")]public Vector3 recoilRotationAim;
        [ConditionalHide("useRecoil")]public Vector3 recoilAmountAim;
        [Space]
        [ConditionalHide("useWeaponSway")]public float drag = 2.5f;
        [ConditionalHide("useWeaponSway")]public float dragThreshold = -5f;
        [ConditionalHide("useWeaponSway")] public float smooth = 5;
        [Space]
        [ConditionalHide("hasAttatchments")]public Attatchements weaponAttatchments;
        public Ammocounter ammoCounter;

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

        public int SubtractClipAmmo(int ammountToRemove)
        {
            return s_ClipAmmo = s_ClipAmmo - ammountToRemove;
        }

        /// <summary>
        /// Depletes ammo upon shooting!
        /// </summary>
        public int SetUsedClipAmmo(int ammoToSubtract)
        {
            ammoCounter.AnimPlay();
            return s_currentAmmo = s_currentAmmo - ammoToSubtract;
        }

        public int SetCurrentClipAmmo()
        {
            return s_currentAmmo = s_currentAmmo + clipSize;
        }
    }
}
#pragma warning restore IDE0090