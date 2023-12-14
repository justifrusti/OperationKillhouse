using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunRangeTarget : MonoBehaviour
{
    public List<GameObject> bulletHoles;
    public GameObject bulletHole;
    public Animation resetAnimation;
    public Transform frontLimit;
    public Transform backLimit;
    public float moveSpeed;
    public bool callTargetBool;
    public bool moveTargetBackBool;

    void Update()
    {
        if (callTargetBool)
            CallTarget();
        
        if(moveTargetBackBool)
            MoveTargetBack();
    }

    void RemoveBulletHoles()
    {
        for (int i = 0; i < bulletHoles.Count; i++)
        {
            Destroy(bulletHoles[i]);
            bulletHoles.RemoveAt(i);
        }
    }

    void CallTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, frontLimit.position, moveSpeed * Time.deltaTime);
    }

    void MoveTargetBack()
    {
        transform.position = Vector3.MoveTowards(transform.position, backLimit.position, moveSpeed * Time.deltaTime);
    }

    public void SetCallTarget(bool call)
    {
        callTargetBool = call;
    }

    public void SetMoveBack(bool move)
    {
        moveTargetBackBool = move;
    }
}
