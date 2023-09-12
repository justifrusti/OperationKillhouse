using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gun
{
    public class GunManager : MonoBehaviour
    {
        public GunProperties gunProperties = new GunProperties();

        public void Fire()
        {
            if(gunProperties.GetCurrentAmmo() > 0)
            {
                //Fire Animation!

                //Implement Ammo Consumption
            }else
            {
                //Implement Reload
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

        //s_Info (DO NOT CHANGE IN CODE!)
        [Tooltip("The current gun ammo, DO NOT CHANGE THIS NUMBER DIRECTLY IN CODE!")]private int s_currentAmmo;
        [Tooltip("The ammount of ammo the gun has left for reloading, DO NOT CHANGE THIS NUMBER DIRECTLY IN CODE")]private int s_ClipAmmo;

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

            return ammoToReturn;
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
            return s_ClipAmmo + ammoToAdd;
        }
    }
}
