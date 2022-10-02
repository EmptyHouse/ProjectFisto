using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EHMovementComponent), typeof(EHDamageableComponent), typeof(EHAttackComponent))]
[RequireComponent(typeof(EHBoxCollider2D), typeof(EHHitboxComponent))]
public class EHCharacter : EHActor
{
    [SerializeField]
    private string CharacterId;
    public EHMovementComponent MovementComponent { get; private set; }
    public EHAbilityComponent AbilityComponent { get; private set; }
    

    protected override void Awake()
    {
        base.Awake();
        MovementComponent = GetComponent<EHMovementComponent>();
        AbilityComponent = GetComponent<EHAbilityComponent>();
    }

    public virtual void SetUpControllerInput(EHPlayerController PlayerController)
    {
        
    }

    public void FreezeActorForSeconds(float FreezeTime)
    {
    }

    // private IEnumerator FreezeActorCoroutine(float FreezeTime)
    // {
    //     
    //     float TimeThatHasPassed = 0;
    //         
    // }
}
