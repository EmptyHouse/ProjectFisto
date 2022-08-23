using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChargeAbility", menuName = "GameplayAbilities/ChargeAbility", order = 1)]
public class EHChargeAbility : EHGameplayAbility
{
    [Header("Charge Properties")] 
    [SerializeField]
    private float MaxChargeTime;
    [SerializeField, Tooltip("The animation that will loop while our character is charging")]
    private AnimationClip ChargingClip;
    

    private int ChargingAnimationHash;
    private float CurrentChargeTime;
    private bool IsCharging;
    

    #region override functions
    public override void InitializeAbility(EHActor AbilityOwner)
    {
        base.InitializeAbility(AbilityOwner);
        ChargingAnimationHash = Animator.StringToHash(ChargingClip.name);
    }

    public override void BeginAbility()
    {
        CurrentFramesActive = 0;
        CurrentChargeTime = 0;
        OwnerAnimator.StartAnimationClip(ChargingAnimationHash);
        IsCharging = true;
        if (OwnerMovementComponent != null)
        {
            OwnerMovementComponent.SetIgnorePlayerInput(true);
        }
    }

    public override void TickAbility()
    {
        if (IsCharging)
        {
            if (CurrentChargeTime >= MaxChargeTime)
            {
                ChargeToAttack();
            }
            else
            {
                CurrentChargeTime += EHTime.DeltaTime;
            }
        }
        else
        {
            base.TickAbility();
        }
    }

    public override void ActivateAbility(bool IsActive)
    {
        if (!IsActive && IsCharging)
        {
            ChargeToAttack();
            
        }
    }
    #endregion override functions

    private void ChargeToAttack()
    {
        IsCharging = false;
        OwnerAnimator.StartAnimationClip(AbilityClipHash);
    }
    
    public float GetChargePercent()
    {
        return Mathf.Clamp(CurrentChargeTime / MaxChargeTime, 0f, 1f);
    }
}
