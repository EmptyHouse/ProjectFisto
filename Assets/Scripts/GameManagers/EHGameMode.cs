using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHGameMode : MonoBehaviour
{
    public EHPhysics2DManager PhysicsManager { get; private set; }
    public EHHitboxManager HitboxManager { get; private set; }
    
    #region monobehaviour methods

    protected virtual void Awake()
    {
        PhysicsManager = new EHPhysics2DManager();
        HitboxManager = new EHHitboxManager();
    }

    public virtual void Update()
    {
        PhysicsManager.UpdatePhysicsLoop(EHTime.DeltaTime);
    }

    #endregion 
}
