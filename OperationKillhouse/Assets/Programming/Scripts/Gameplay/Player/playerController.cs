using Gun;
using System;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class playerController : MonoBehaviour
    {
        [Serializable]
        public class AnimatorEvents
        {
            public GunManager gunManager;

            public Animator armAnimator;
            public Animator gunAnimator;

            public void Fire()
            {
                gunManager.aiming = !gunManager.aiming;

                bool a_BoolState = armAnimator.GetBool("Fire");
                bool g_BoolState = gunAnimator.GetBool("Fire");

                armAnimator.SetBool("Fire", !a_BoolState);
                gunAnimator.SetBool("Fire", !g_BoolState);
            }

            public void Reload()
            {
                armAnimator.SetTrigger("Reload");
                gunAnimator.SetTrigger("Reload");
            }

            public void IdleCheck()
            {
                armAnimator.SetTrigger("Check");
                gunAnimator.SetTrigger("Check");
            }

            public void HolsterWeapon()
            {
                armAnimator.SetTrigger("Holster");
                gunAnimator.SetTrigger("Holster");
            }

            public void EmptyMag()
            {
                if(gunManager.gunProperties.GetCurrentAmmo() <= 0)
                {
                    armAnimator.SetBool("Empty", true);
                    gunAnimator.SetBool("Empty", true);
                }else
                {

                    armAnimator.SetBool("Empty", false);
                    gunAnimator.SetBool("Empty", false);

                }
            }
        }

        public enum GameState
        {
            Play, Pause
        }

        public GameState gameState = GameState.Play;

        public AnimatorEvents animEvent;

        public Inputmanager inputmanager;
        public Rigidbody rb;
        public Animation walk;
        public bool inMenu;


        [Header("Movement Values")]
        [Tooltip("The walking speed of the player")] public float walkSpeed;
        [Tooltip("The running speed of the player")] public float runspeed;
        [Tooltip("The acceleration of the player when moving")] public float accel;
        [Tooltip("The force added to the player when jumping")]public int jumpforce;
        float currentRunspeed;
        bool isGrounded;
        bool leaning;
        float speed;

        [Header("Crouching values")]
        [Tooltip("Allows the player to crouch")]public bool canCrouch;
        [ConditionalHide("canCrouch")][Tooltip("The heigt of the player when crouching 1 is normal hieght")]public float crouchHeight;
        private float normalHeight;

        [Header("Leaning values")]
        [Tooltip("Allows the player to lean")]public bool canLean;
        [ConditionalHide("canLean")][Tooltip("The speed you lean at")]public float leanSpeed;
        [ConditionalHide("canLean")][Tooltip("The distance you lean to the side")]public float leanDistance;
        [ConditionalHide("canLean")][Tooltip("The height of the camera when you lean")]public float leanHight;
        [ConditionalHide("canLean")][Tooltip("The amount the camera rotates leave 0 if you dont want to rotate the camera when leaning")]public float leanAngle;
        private Vector3 targetLeanPos;
        private float currentLeanAngle;
        private float targetLeanAngle;
        private float leanInput;
        private float baseHight;

        [Header("Camera sensetivity")]
        [Tooltip("The sensetivity of the camera")]public float sensetivity;
        public Transform cam;
        [Space]


        public bool useCameraDelay;
        [ConditionalHide("useCameraDelay")]public Transform weaponRotPoint;
        [ConditionalHide("useCameraDelay")] public float yWeaponRotLimit;
        [ConditionalHide("useCameraDelay")] public float xWeaponRotLimit;
        [ConditionalHide("useCameraDelay")] public float weaponRotSpeed;

        private float xRotation = 0;
        private float yWeaponRotation = 0;
        private float xWeaponRotation = 0;

        [Space]
        //public GameObject hole;
        public RaycastHit hit;

        public GameObject generationUI;

        public GameObject primarygun;
        public GameObject secondarygun;

        public GameObject armory;

        Vector3 rot;

        private void Start()
        {
            inputmanager.inputMaster.Movement.Jump.started += _ => Jump();

            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
            baseHight = cam.transform.localPosition.y;
            normalHeight = GetComponent<CapsuleCollider>().height;
        }
        
        private void Update()
        {
            switch (gameState)
            {
                case GameState.Play:

                    Cursor.lockState = CursorLockMode.Locked;

                    float forwardBackward = inputmanager.inputMaster.Movement.ForwardBackward.ReadValue<float>();
                    float leftRight = inputmanager.inputMaster.Movement.RightLeft.ReadValue<float>();
                    Vector3 move = transform.right * leftRight  + transform.forward * forwardBackward;

                    if (speed <= walkSpeed && forwardBackward != 0 | leftRight != 0)
                    {
                        speed += accel;
                    }
                    else if(speed >= 0)
                    {
                        speed -= accel;
                    }

                    if (currentRunspeed <= runspeed && inputmanager.inputMaster.Movement.Sprint.ReadValue<float>() == 1 && forwardBackward != 0 | leftRight != 0)
                    {
                        currentRunspeed += accel * 2;
                    }
                    else if(currentRunspeed >= 0)
                    {
                        currentRunspeed -= accel;
                    }

                    move *= inputmanager.inputMaster.Movement.Sprint.ReadValue<float>() == 0 ? speed : currentRunspeed;

                    rb.velocity = new Vector3(move.x,rb.velocity.y, move.z);

                    Vector2 mouseV2 = inputmanager.inputMaster.CameraMovement.MouseX.ReadValue<Vector2>() * sensetivity * Time.deltaTime;

                    xRotation -= mouseV2.y;
                    xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                    cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

                    if(!animEvent.gunManager.aiming && !leaning && useCameraDelay)
                    {
                        if (mouseV2.y < 0.5f || mouseV2.y < -0.5f)
                        {
                            xWeaponRotation += mouseV2.y * weaponRotSpeed;
                            xWeaponRotation = Mathf.Clamp(xWeaponRotation, -xWeaponRotLimit, xWeaponRotLimit);
                        }

                        if (mouseV2.x < 0.5f || mouseV2.x < -0.5f)
                        {
                            yWeaponRotation += mouseV2.x * weaponRotSpeed;
                            yWeaponRotation = Mathf.Clamp(yWeaponRotation, -yWeaponRotLimit, yWeaponRotLimit);
                        }

                        weaponRotPoint.localRotation = Quaternion.Euler(-xWeaponRotation, yWeaponRotation, 0f);
                    }

                    transform.Rotate(Vector3.up * mouseV2.x);

                    if (canLean && inputmanager.inputMaster.Leaning.LeanLeft.ReadValue<float>() == 1)
                    {
                        if (!Physics.Raycast(cam.transform.position, -transform.right, out hit, 2))
                        {
                            if (hit.transform == null)
                            {
                                leaning = true;
                                targetLeanPos.x = -leanDistance;
                                targetLeanPos.y = leanHight;
                                leanInput = 1f;
                                Leaning();
                            }
                        }
                        else
                        {
                            leaning = false;
                            targetLeanPos.x = 0;
                            targetLeanPos.y = baseHight;
                            leanInput = 0f;
                            Leaning();
                        }
                    }
                    else if (canLean && inputmanager.inputMaster.Leaning.LeanRight.ReadValue<float>() == 1)
                    {
                        if (!Physics.Raycast(cam.transform.position, transform.right, out hit, 1))
                        {
                            if(hit.transform == null)
                            {
                                leaning = true;
                                targetLeanPos.x = leanDistance;
                                targetLeanPos.y = leanHight;
                                leanInput = -1f;
                                Leaning();
                            }
                        }
                        else
                        {
                            leaning = false;
                            targetLeanPos.x = 0;
                            targetLeanPos.y = baseHight;
                            leanInput = 0f;
                            Leaning();
                        }
                    }
                    else
                    {
                        leaning = false;
                        targetLeanPos.x = 0;
                        targetLeanPos.y = baseHight;
                        leanInput = 0f;
                        Leaning();
                    }

                    if (canCrouch && inputmanager.inputMaster.Movement.Crouch.ReadValue<float>() == 1)
                    {
                        float newHeight = Mathf.Lerp(GetComponent<CapsuleCollider>().height, crouchHeight, Time.deltaTime * leanSpeed);
                        GetComponent<CapsuleCollider>().height = newHeight;

                        animEvent.gunManager.gunProperties.recoilPos.localPosition = Vector3.Slerp(animEvent.gunManager.gunProperties.recoilPos.localPosition, animEvent.gunManager.gunProperties.crouchPosOffset, leanSpeed * Time.deltaTime);
                        rot = Vector3.Slerp(rot, animEvent.gunManager.gunProperties.crouchRotOffset, leanSpeed * Time.deltaTime);
                        animEvent.gunManager.gunProperties.recoilPos.localRotation = Quaternion.Euler(rot);
                    }
                    else
                    {
                        float newHeight = Mathf.Lerp(GetComponent<CapsuleCollider>().height, normalHeight, Time.deltaTime * leanSpeed);
                        GetComponent<CapsuleCollider>().height = newHeight;

                        animEvent.gunManager.gunProperties.recoilPos.localPosition = Vector3.Slerp(animEvent.gunManager.gunProperties.recoilPos.localPosition, new Vector3(0,0,0), leanSpeed * Time.deltaTime);
                        rot = Vector3.Slerp(rot, new Vector3(0, 0, 0), leanSpeed * Time.deltaTime);
                        animEvent.gunManager.gunProperties.recoilPos.localRotation = Quaternion.Euler(rot);
                    }

                    animEvent.EmptyMag();

                    if (Input.GetButtonDown("Fire1"))
                    {
                        if (animEvent.gunManager.gunProperties.GetCurrentAmmo() > 0)
                        {
                            animEvent.Fire();
                        }
                    }
                    else if(Input.GetButtonUp("Fire1"))
                    {
                        if (animEvent.gunManager.gunProperties.GetClipAmmo() > 0)
                        {
                            animEvent.Fire();
                        }
                    }

                    if (animEvent.gunManager.gunProperties.GetClipAmmo() <= 0)
                    {
                        animEvent.armAnimator.SetBool("Fire", false);
                        animEvent.gunAnimator.SetBool("Fire", false);
                    }

                    if (Input.GetKeyDown (KeyCode.C))
                    {
                        animEvent.armAnimator.SetTrigger ("Check");
                        animEvent.gunAnimator.SetTrigger ("Check");
                    }

                    if (Input.GetButtonDown("Fire2"))
                    {
                        animEvent.gunAnimator.SetBool("Aim", true);
                        animEvent.armAnimator.SetBool("Aim", true);
                    }
                    else if(Input.GetButtonUp("Fire2"))
                    {
                        animEvent.gunAnimator.SetBool("Aim", false);
                        animEvent.armAnimator.SetBool("Aim", false);
                    }

                    if (Input.GetKeyDown(KeyCode.Alpha1) && !primarygun.activeSelf || Input.GetKeyDown(KeyCode.Alpha2) && !secondarygun.activeSelf)
                    {
                        animEvent.armAnimator.SetTrigger("Holster");
                        animEvent.gunAnimator.SetTrigger("Holster");
                    }


                    if(Input.GetKeyDown(KeyCode.R))
                    {
                        animEvent.Reload();
                    }

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        RaycastHit hit;
                        if(Physics.Raycast(cam.transform.position, cam.forward, out hit, 500f))
                        {
                            if(hit.transform.gameObject.tag == "Armory")
                            {
                                armory.SetActive(true);
                                ChangeGameState (GameState.Pause);
                                primarygun.SetActive (false);   
                            }

                            if(hit.transform.gameObject.tag == "Range Target")
                            {
                                hit.transform.GetComponent<GunRangeTarget>().resetAnimation.Play();
                            }
                        }
                    }

                    break;

                case GameState.Pause:

                    Cursor.lockState = CursorLockMode.None;
                    break;
            }

           /* if(inputmanager.inputMaster.Pause.PauseKey.ReadValue<float>() == 1)
            {
                ChangeGameState(GameState.Pause);
            }*/

            if (Input.GetKeyDown(KeyCode.G))
            {
                if(generationUI.activeSelf == false)
                {
                    generationUI.SetActive(true);
                    ChangeGameState (GameState.Pause);
                }
                else
                {
                    generationUI.SetActive(false);
                    ChangeGameState (GameState.Play);
                }
            }
        }


        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.CompareTag("Ground"))
                isGrounded = true;
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.transform.CompareTag("Ground"))
                isGrounded = false;
        }

        void Jump()
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpforce);
            }
        }

        void Leaning()
        {
            targetLeanAngle = leanInput * leanAngle;
            currentLeanAngle = Mathf.Lerp(currentLeanAngle, targetLeanAngle, Time.deltaTime * leanSpeed);

            cam.localRotation = Quaternion.Euler(xRotation, cam.localRotation.y, currentLeanAngle);
            cam.localPosition = Vector3.Lerp(cam.localPosition, targetLeanPos, Time.deltaTime * leanSpeed);

        }

        public void WeaponSwapping()
        {
            if (!primarygun.activeSelf)
            {
                secondarygun.SetActive(false);
                primarygun.SetActive(true);
                animEvent.armAnimator = GameObject.FindGameObjectWithTag("Arm").GetComponent<Animator>();
                animEvent.gunAnimator = GameObject.FindGameObjectWithTag("Gun").GetComponent<Animator>();
                animEvent.gunManager = GetComponentInChildren<GunManager>();
            }
            else if (!secondarygun.activeSelf)
            {
                primarygun.SetActive(false);
                secondarygun.SetActive(true);
                animEvent.armAnimator = GameObject.FindGameObjectWithTag("Arm").GetComponent<Animator>();
                animEvent.gunAnimator = GameObject.FindGameObjectWithTag("Gun").GetComponent<Animator>();
                animEvent.gunManager = GetComponentInChildren<GunManager>();
            }

        }

        public void ReturnToPlay()
        {
            ChangeGameState(GameState.Play);
        }

        public void ChangeGameState(GameState newState)
        {
            gameState = newState;
        }
        
    }
}
