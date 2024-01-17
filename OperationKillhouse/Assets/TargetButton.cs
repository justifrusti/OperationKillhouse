using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetButton : MonoBehaviour
{
    public GunRangeTarget target;

    public void TargetLogic()
    {
        if (this.name == ("Reset"))
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
}
