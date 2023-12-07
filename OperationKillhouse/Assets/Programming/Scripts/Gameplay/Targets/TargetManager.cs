using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public List<GameObject> redTargets;
    public List<GameObject> blueTargets;
    Generator generator;
    int s_BlueTargetsHit;
    bool targetsFound;

    void Start()
    {
        generator = GameObject.FindGameObjectWithTag("Generator").GetComponent<Generator>();
    }

    void Update()
    {
        if(generator != null && generator.dungeonGenerationComplete && !targetsFound)
        {
            var foundRedTargets = GameObject.FindGameObjectsWithTag("Red Target");

            for(int i = 0; i < foundRedTargets.Length - 1; i++)
            {
                if (!redTargets.Contains(foundRedTargets[i]))
                {
                    redTargets.Add(foundRedTargets[i]);
                }

                if (redTargets[i] == null)
                {
                    redTargets.Remove(foundRedTargets[i]);
                }
            }

            var foundBlueTargets = GameObject.FindGameObjectsWithTag("Blue Target");

            for (int i = 0; i < foundBlueTargets.Length - 1; i++)
            {
                if (!blueTargets.Contains(foundBlueTargets[i].gameObject))
                {
                    blueTargets.Add(foundBlueTargets[i]);
                }

                if (blueTargets[i] == null)
                {
                    blueTargets.Remove(foundBlueTargets[i]);
                }
            }
            targetsFound = true;
        }

        for(int i = 0;i < redTargets.Count;i++) 
        {
            if (redTargets[i] == null)
            {
                redTargets.Remove(redTargets[i]);
            }
        }

        for (int i = 0; i < blueTargets.Count; i++)
        {
            if (blueTargets[i] == null)
            {
                blueTargets.Remove(blueTargets[i]);
            }
        }
    }

    //setters\\
    public void SetBlueTargets(int blueTargetsToSet)
    {
        s_BlueTargetsHit = blueTargetsToSet;
    }

    public void AddToBlueTargets()
    {
        s_BlueTargetsHit++;
    }

    //Getters\\

    public int GetBlueTargets()
    {
        return s_BlueTargetsHit;
    }
}
