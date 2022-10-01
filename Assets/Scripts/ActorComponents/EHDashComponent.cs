using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHDashComponent : EHActorComponent
{
    #region const varaibles

    private readonly int Anim_DashTrigger = Animator.StringToHash("Roll");
    #endregion const variables
    
    public float DashSpeed = 10;
    public float DashDuration = 1;
    private float TimeRemainingForDash;

    private float CurrentActiveDuration;
    private EHMovementComponent MovementComponent;
    private EHAnimatorComponent AnimatorComponent;
    private EHPhysics2D Physics;

    protected override void Awake()
    {
        base.Awake();
        MovementComponent = GetComponent<EHMovementComponent>();
        Physics = GetComponent<EHPhysics2D>();
        AnimatorComponent = GetComponent<EHAnimatorComponent>();
        this.enabled = false;
    }

    public void AttemptDash()
    {
        AnimatorComponent.SetTrigger(Anim_DashTrigger);
    }

    private void Update()
    {
        UpdateDash();
        TimeRemainingForDash -= EHTime.DeltaTime;
        if (TimeRemainingForDash < 0)
        {
            OnDashEnd();
        }
    }

    public void OnBeginDash()
    {
        this.enabled = true;
        TimeRemainingForDash = DashDuration;
    }

    private void UpdateDash()
    {
        Vector2 DashVelocity = new Vector2()
        {
            x = Mathf.Sign(GetActorScale().x) * DashSpeed,
            y = Physics.Velocity.y
        };
        
        Physics.SetVelocity(DashVelocity);
    }

    private void OnDashEnd()
    {
        this.enabled = false;
        
        Physics.SetVelocity(new Vector2()
        {
            x = Mathf.Sign(GetActorScale().x) * 
                MovementComponent.GetMaxSpeedFromMovementStance(EMovementStance.Standing),
            y = Physics.Velocity.y,
        });
    }
}
