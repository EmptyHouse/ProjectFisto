using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct FAttackData
{
    // The actor component applying the hit data
    public EHAttackComponent Owner;
    // The raw damage applied to the damage component
    public int DamageApplied;
    // Force direction that will be applied to the damaged component
    public Vector2 DamageForce;
}

public class EHAttackComponent : EHActorComponent
{
    
}
