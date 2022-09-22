using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct FAttackData
{
    // The raw damage applied to the damage component
    public int DamageApplied;
    // Force direction that will be applied to the damaged component
    public Vector2 DamageForce;
}

public enum EAttackType
{
    SimpleAttack,
    Dash,
}

public class EHAttackComponent : EHActorComponent
{
    private readonly int Anim_AttackTrigger = Animator.StringToHash("Attack");
    private const float AttackBufferTime = EHTime.TimePerFrame * 8;
    
    private EHAnimatorComponent Anim;
    [SerializeField] 
    private FAttackData DefaultAttackData;
    [SerializeField]
    private float ChargeReleaseSpeed = 10f;


    private EHMovementComponent OwnerMovementComponent;
    
    #region monobehaviour methods
    protected override void Awake()
    {
        base.Awake();
        Anim = AssociatedActor.Anim;
        OwnerMovementComponent = AssociatedActor.GetComponent<EHMovementComponent>();
    }

    #endregion monobehaviour methods

    public void AttackPressed()
    {
        Anim.SetTrigger(Anim_AttackTrigger, AttackBufferTime);
    }

    public void AttackDamageComponent(EHDamageableComponent OtherDamageComponent)
    {
        EHActor OtherAssociatedActor = OtherDamageComponent.AssociatedActor;
        if (OtherAssociatedActor == GetOwningActor())
        {
            return;//do not apply damage to owner
        }
        OtherDamageComponent.TakeDamage(DefaultAttackData);
    }
}
