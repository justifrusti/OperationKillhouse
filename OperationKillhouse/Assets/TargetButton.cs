using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetButton : MonoBehaviour
{
    public GunRangeTarget target;
    public Material LitUpMat;
    public Material litOffMat;
    public GameObject interactText;

    public void FixedUpdate()
    {
        target.callTargetBool = false;
        target.moveTargetBackBool = false;
        interactText.SetActive(false);
        LightOff();
    }

    public void TargetLogic()
    {
        interactText.SetActive(true);
        if (this.name == ("ResetButton"))
        {
            target.resetAnimation.Play();
        }

        if (this.name == ("Call"))
        {
            target.callTargetBool = true;
        }

        if (this.name == ("Back"))
        {
            target.moveTargetBackBool = true;
        }
    }

    public void LightUp()
    {
        this.GetComponent<Renderer>().material = LitUpMat;
    }

    public void LightOff()
    {
        this.GetComponent<Renderer>().material = litOffMat;
    }
}
