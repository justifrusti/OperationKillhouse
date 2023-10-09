using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int health = 1;
    public Animation animationl;
    bool animPlayed;

    public void Damage(int damage)
    {
        if((health -= damage) <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        animationl.Play();
        //score shit for non ennemy
        //and ennemy stuff
        animPlayed = true;
    }


}
