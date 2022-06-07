using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHCharacter : EHActor
{
    public EHPhysics2D Physics { get; private set; }
    

    protected override void Awake()
    {
        base.Awake();
        Physics = GetComponent<EHPhysics2D>();
    }

    public virtual void SetUpControllerInput(EHPlayerController PlayerController)
    {
        
    }
}
