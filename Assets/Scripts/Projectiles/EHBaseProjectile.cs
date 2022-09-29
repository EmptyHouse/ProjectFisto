using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHBaseProjectile : EHActor
{
    [SerializeField]
    private bool ShouldRotateProjectile;
    
    public void LaunchProjectile(Vector2 LaunchVelocity)
    {
        if (Physics)
        {
            Physics.SetVelocity(LaunchVelocity);
        }
    }
}
