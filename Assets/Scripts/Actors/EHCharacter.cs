using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EHMovementComponent), typeof(EHDamageableComponent), typeof(EHAttackComponent))]
[RequireComponent(typeof(EHBoxCollider2D), typeof(EHHitboxComponent))]
public class EHCharacter : EHActor
{
    [SerializeField]
    private string CharacterId;
    public EHPhysics2D Physics { get; private set; }
    public EHMovementComponent MovementComponent { get; private set; }
    public EHDamageableComponent DamageableComponent { get; private set; }
    public EHAttackComponent AttackComponent { get; private set; }
    public EHHitboxComponent HitboxComponent { get; private set; }
    

    protected override void Awake()
    {
        base.Awake();
        Physics = GetComponent<EHPhysics2D>();
        MovementComponent = GetComponent<EHMovementComponent>();
        DamageableComponent = GetComponent<EHDamageableComponent>();
        AttackComponent = GetComponent<EHAttackComponent>();
        HitboxComponent = GetComponent<EHHitboxComponent>();
    }

    public virtual void SetUpControllerInput(EHPlayerController PlayerController)
    {
        
    }
}
