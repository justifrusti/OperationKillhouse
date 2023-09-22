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
            CheckAttatcment();
        }

        public void CheckAttatcment()
        {
            switch(type)
            {
                case AttatchmentType.ForeGrip:
                    gunManager.gunProperties.weaponAttatchments.SetForeGrip(gunManager.gunProperties.weaponAttatchments.foreGrip);

                    if (attatchementID == gunManager.gunProperties.weaponAttatchments.GetForeGripID())
                    {
                        attatchmentObj.SetActive(true);
                    }
                    else
                    {
                        DisableAttatchment();
                    }
                    break;

                case AttatchmentType.Optics:
                    gunManager.gunProperties.weaponAttatchments.SetOptic(gunManager.gunProperties.weaponAttatchments.optic);

                    if (attatchementID == gunManager.gunProperties.weaponAttatchments.GetOpticsID())
                    {
                        attatchmentObj.SetActive(true);
                    }
                    else
                    {
                        DisableAttatchment();
                    }
                    break;

                case AttatchmentType.BackupOptics:
                    gunManager.gunProperties.weaponAttatchments.SetBackupOptics(gunManager.gunProperties.weaponAttatchments.backupOptics);

                    if (attatchementID == gunManager.gunProperties.weaponAttatchments.GetBackupOpticsID())
                    {
                        attatchmentObj.SetActive(true);
                    }
                    else
                    {
                        DisableAttatchment();
                    }
                    break;

                case AttatchmentType.MuzzleDevice:
                    gunManager.gunProperties.weaponAttatchments.SetMuzzleDevices(gunManager.gunProperties.weaponAttatchments.muzzleDevices);

                    if (attatchementID == gunManager.gunProperties.weaponAttatchments.GetMuzzleDevicesID())
                    {
                        attatchmentObj.SetActive(true);
                    }
                    else
                    {
                        DisableAttatchment();
                    }
                    break;

                case AttatchmentType.Flashlight:
                    gunManager.gunProperties.weaponAttatchments.SetFlashlights(gunManager.gunProperties.weaponAttatchments.flashlights);

                    if (attatchementID == gunManager.gunProperties.weaponAttatchments.GetFlashlightsID())
                    {
                        attatchmentObj.SetActive(true);
                    }
                    else
                    {
                        DisableAttatchment();
                    }
                    break;

                case AttatchmentType.Lasersight:
                    gunManager.gunProperties.weaponAttatchments.SetFlashlights(gunManager.gunProperties.weaponAttatchments.flashlights);

                    if (attatchementID == gunManager.gunProperties.weaponAttatchments.GetFlashlightsID())
                    {
                        attatchmentObj.SetActive(true);
                    }
                    else
                    {
                        DisableAttatchment();
                    }
                    break;

                case AttatchmentType.Buttstock:
                    gunManager.gunProperties.weaponAttatchments.SetButtstocks(gunManager.gunProperties.weaponAttatchments.buttstocks);

                    if (attatchementID == gunManager.gunProperties.weaponAttatchments.GetButtStocksID())
                    {
                        attatchmentObj.SetActive(true);
                    }
                    else
                    {
                        DisableAttatchment();
                    }
                    break;
            }
        }

        public void DisableAttatchment()
        {
            attatchmentObj.SetActive(false);
        }
    }
}
