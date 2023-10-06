using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int health = 1;
    public Animation animationl;
    public bool enemy, civilian;
    bool animPlayed;

    // Update is called once per frame
    void Update()
    {
        if(health <= 0 && !animPlayed)
            Death();
    }

    void Death()
    {
        animationl.Play();
        //score shit for non ennemy
        //and ennemy stuff
        animPlayed = true;
    }


}
