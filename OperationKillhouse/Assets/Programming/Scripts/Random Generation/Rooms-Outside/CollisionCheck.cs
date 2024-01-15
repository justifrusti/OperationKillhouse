
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    /*[HideInInspector]*/public List<GameObject> collidersFound;
    bool collEnabled = false;
    bool collClear = false;

    public Collider[] checkColliers;

    float destroyTimer = .1f;

    private void Awake()
    {
        enableColliders();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collidersFound.Contains(collision.gameObject))
        {
            collidersFound.Add(collision.gameObject);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < collidersFound.Count; i++)
        {
            if (collidersFound[i] == null)
            {
                collidersFound.Remove(collidersFound[i]);
            }
        }
        
        if(collidersFound.Count == 0 && collEnabled)
        {
            collClear = true;
        }
        else if (collEnabled)
        {
            collClear = false;
        }



        if (checkColliers[checkColliers.Length -1].enabled == true)
        {
            collEnabled = true;
        }

        if (destroyTimer > 0)
        {
            destroyTimer -= Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void enableColliders()
    {
        for (int i = 0; i < checkColliers.Length; i++)
        {
            checkColliers[i].enabled = true;
        }
    }

    public bool GetCollEnabled()
    {
        return collEnabled;
    }

    public bool GetCollClear()
    {
        return collClear;
    }

    public int GetCollLenght()
    {
        return collidersFound.Count;
    }
}
