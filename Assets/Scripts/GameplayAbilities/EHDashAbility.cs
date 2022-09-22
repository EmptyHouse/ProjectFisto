using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashAbility", menuName = "GameplayAbilities/DashAbility", order = 1)]
public class EHDashAbility : EHBaseGameplayAbility
{
    [SerializeField]
    private float DashSpeed = 25f;
    
    [SerializeField]
    private float DashDuration;

    private float TotalDashTimePassed;

    public override void BeginAbility()
    {
        base.BeginAbility();
        TotalDashTimePassed = 0;
        if (OwnerPhysicsComponent)
        {
            OwnerPhysicsComponent.SetVelocity(new Vector2(Mathf.Sign(AbilityOwner.GetScale().x) * DashSpeed, 0));
            OwnerPhysicsComponent.SetUseGravity(false);
        }
    }

    public override void TickAbility(float DeltaSeconds)
    {
        base.TickAbility(DeltaSeconds);
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
                OwnerPhysicsComponent.SetUseGravity(true);

            }
        }
    }
}