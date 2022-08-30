using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashAbility", menuName = "GameplayAbilities/DashAbility", order = 1)]
public class EHDashAbility : EHGameplayAbility
{
    [SerializeField]
    private float DashSpeed;
    
    [SerializeField]
    private float DashDuration;

    private float TotalDashTimePassed;

    private void OnValidate()
    {
        if (AbilityClips.Count > 0)
        {
            if (DashDuration > AbilityClips[0].length)
            {
                DashDuration = AbilityClips[0].length;
            }
        }
    }

    public override void BeginAbility()
    {
        base.BeginAbility();
        TotalDashTimePassed = 0;
        if (OwnerPhysicsComponent)
        {
            OwnerPhysicsComponent.SetVelocity(new Vector2(Mathf.Sign(AbilityOwner.GetScale().x) * DashSpeed, 0));
        }
    }

    public override void TickAbility()
    {
        base.TickAbility();
        float PreviousTime = TotalDashTimePassed;
        TotalDashTimePassed += EHTime.DeltaTime;
        if (PreviousTime < DashDuration && TotalDashTimePassed >= DashDuration)
        {
            if (OwnerMovementComponent && AbilityOwner && OwnerPhysicsComponent)
            {
                float MaxVelocity = Mathf.Sign(AbilityOwner.GetScale().x) * 
                                    OwnerMovementComponent.GetMaxSpeedFromMovementStance(OwnerMovementComponent.MovementStance);
                Vector2 Velocity = OwnerPhysicsComponent.Velocity;
                Velocity.x = MaxVelocity;
                OwnerPhysicsComponent.SetVelocity(Velocity);
            }
        }
    }
}
