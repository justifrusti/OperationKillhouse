using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gun
{
    public class Attatchement : MonoBehaviour
    {
        public enum AttatchmentType { ForeGrip, BackupOptics, Optics, MuzzleDevice, Flashlight, Lasersight, Buttstock }

        public GunManager gunManager;

        public int attatchementID;
        public AttatchmentType type;
        public GameObject attatchmentObj;

        private void Start()
        {
            switch(type)
            {
                case AttatchmentType.ForeGrip:
                    CheckForeGrip();
                    break;
            }
        }

        public void CheckForeGrip()
        {
            gunManager.gunProperties.weaponAttatchments.SetForeGrip(gunManager.gunProperties.weaponAttatchments.foreGrip);

            if (attatchementID == gunManager.gunProperties.weaponAttatchments.GetForeGripID())
            {
                attatchmentObj.SetActive(true);
            }
            else
            {
                DisableForeGrip();
            }

            Debug.Log(gunManager.gunProperties.weaponAttatchments.GetForeGripID());
        }

        public void DisableForeGrip()
        {
            attatchmentObj.SetActive(false);
        }
    }
}
