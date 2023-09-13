using System;
using UnityEngine;

namespace Player
{
    public class playerController : MonoBehaviour
    {
        public Inputmanager inputmanager;

        public Rigidbody rb;
        public Collider collider;

        [Header("Movement Values")]
        [Tooltip("The walking speed of the player")] public float walkSpeed;
        [Tooltip("The running speed of the player")] public float runspeed;
        [Tooltip("The acceleration of the player when moving")] public float accel;
        [Tooltip("The force added to the player when jumping")]public int jumpforce;
        [Tooltip("The heigt of the player when crouching 1 is normal hieght")]public float crouchHeight;
        float currentRunspeed;
        bool isGrounded;
        float speed;

        [Header("Leaning values")]
        [Tooltip("The speed you lean at")]public float leanSpeed;
        [Tooltip("The distance you lean to the side")]public float leanDistance;
        [Tooltip("The height of the camera when you lean")]public float leanHight;
        private Vector3 targetLeanPos;

        [Header("Camera sensetivity")]
        [Tooltip("The sensetivity of the camera")]public float sens;
        Transform cam;

        private float xRotation = 0;

        private void Start()
        {
            inputmanager.inputMaster.Movement.Jump.started += _ => Jump();
            Cursor.lockState = CursorLockMode.Locked;
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        }
        
        private void Update()
        {
            float forwardBackward = inputmanager.inputMaster.Movement.ForwardBackward.ReadValue<float>();
            float leftRight = inputmanager.inputMaster.Movement.RightLeft.ReadValue<float>();
            Vector3 move = transform.right * leftRight  + transform.forward * forwardBackward;

            if (speed <= walkSpeed && forwardBackward != 0 | leftRight != 0)
                speed += accel;
            else if(speed >= 0) speed -= accel;

            if (currentRunspeed <= runspeed && inputmanager.inputMaster.Movement.Sprint.ReadValue<float>() == 1 && forwardBackward != 0 | leftRight != 0)
                currentRunspeed += accel * 2;
            else if(currentRunspeed >= 0) currentRunspeed -= accel;

            move *= inputmanager.inputMaster.Movement.Sprint.ReadValue<float>() == 0 ? speed : currentRunspeed;
            if(inputmanager.inputMaster.Movement.Crouch.ReadValue<float>() == 1)
            {
                float newHeight = Mathf.Lerp(GetComponent<CapsuleCollider>().height, crouchHeight, Time.deltaTime * leanSpeed);
                GetComponent<CapsuleCollider>().height = newHeight;
            }
            else
            {
                float newHeight = Mathf.Lerp(GetComponent<CapsuleCollider>().height, 2, Time.deltaTime * leanSpeed);
                GetComponent<CapsuleCollider>().height = newHeight;
            }
            //transform.localScale = new Vector3(x: 1, y: inputmanager.inputMaster.Movement.Crouch.ReadValue<float>() == 0 ? 1f : crouchHeight, z: 1);

            rb.velocity = new Vector3(move.x,rb.velocity.y, move.z);

            Vector2 mouseV2 = inputmanager.inputMaster.CameraMovement.MouseX.ReadValue<Vector2>() * sens * Time.deltaTime;

            xRotation -= mouseV2.y;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseV2.x);

            if (inputmanager.inputMaster.Leaning.LeanLeft.ReadValue<float>() == 1)
            {
                targetLeanPos.x = -leanDistance;
                targetLeanPos.y = leanHight;
                Leaning();
            }
            else if (inputmanager.inputMaster.Leaning.LeanRight.ReadValue<float>() == 1)
            {
                targetLeanPos.x = leanDistance;
                targetLeanPos.y = leanHight;
                Leaning();
            }
            else
            {
                targetLeanPos.x = 0;
                targetLeanPos.y = 1.75f;
                Leaning();
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
                rb.AddForce(Vector3.up * jumpforce);    
        }

        void Leaning()
        {
            cam.localPosition = Vector3.Lerp(cam.localPosition, targetLeanPos, Time.deltaTime * leanSpeed);

        }
    }

}
