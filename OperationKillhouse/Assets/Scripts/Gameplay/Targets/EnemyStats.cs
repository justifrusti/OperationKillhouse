using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int health = 1;
    public Animation animationl;
    bool death;

    public void Damage(int damage)
    {
        if((health -= damage) <= 0)
        {
            death = true;
        }
    }

    public void Update()
    {
        if (death)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-90, transform.eulerAngles.y, transform.eulerAngles.z), Time.deltaTime * 3);
        }
    }


}
