using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EMovementStance
{
    Standing,
    InAir,
}

[RequireComponent(typeof(EHPhysics2D))]
public class EHMovementComponent : EHCharacterComponent
{
    #region const variables
    // Input minimum before our character moves
    private const float JoystickDeadzone = .25f;
    // Input Minimum before we register a walking movement
    private const float JoystickWalkThreshold = JoystickDeadzone;
    // Input minimum before we register a run movement
    private const float JoystickRunThreshold = .65f;

    private readonly int AnimMovementStance = Animator.StringToHash("MovementStance");
    #endregion const variables

    [SerializeField] 
    private float GroundAcceleration = 50f;
    [SerializeField] 
    private float AirAcceleration = 10f;
    
    [SerializeField]
    private float WalkSpeed = 5;
    [SerializeField]
    private float RunSpeed = 15;
    
    [Header("Jumping Values")]
    [SerializeField]
    private int MaxDoubleJump = 1;
    [SerializeField]
    private float TimeToReachApex = 1;
    [SerializeField]
    private float JumpHeightApex = 1.5f;
    [HideInInspector, SerializeField] 
    private float JumpVelocity;

    private EMovementStance MovementStance = EMovementStance.Standing;
    private EHPhysics2D Physics;
    private int DoubleJumpsUsed;
    
    // Input values
    private Vector2 CurrentInput;
    private Vector2 PreviousInput;
    
    

    #region monobehaviour methods

    protected override void Awake()
    {
        base.Awake();
        Physics = GetComponent<EHPhysics2D>();
    }

    private void OnValidate()
    {
        if (TimeToReachApex != 0)
        {
            if (Physics == null) Physics = GetComponent<EHPhysics2D>();
            
            Physics.GravityScale = (2 * JumpHeightApex) / (EHPhysics2D.GravityConstant * Mathf.Pow(TimeToReachApex, 2));
            JumpVelocity = 2 * JumpHeightApex / TimeToReachApex;
        }
    }

    private void Update()
    {
        UpdateMovementFromInput();
        if (MovementStance != EMovementStance.InAir && Mathf.Abs(Physics.Velocity.y) > 0)
        {
            
        }
        PreviousInput = CurrentInput;
    }

    #endregion monobehaviour methods

    public void SetHorizontalInput(float Input)
    {
        CurrentInput.x = Input;
    }

    public void SetVerticalInput(float Input)
    {
        CurrentInput.y = Input;
    }

    private void UpdateMovementFromInput()
    {
        UpdateVelocityFromInput();
    }

    private void UpdateVelocityFromInput()
    {
        float GoalSpeed;
        float Acceleration;
        float NewSpeed = Physics.Velocity.x;

        switch (MovementStance)
        {
            case EMovementStance.Standing:
                Acceleration = GroundAcceleration;
                if (Mathf.Abs(CurrentInput.x) > JoystickRunThreshold) GoalSpeed = Mathf.Sign(CurrentInput.x) * RunSpeed;
                else if (Mathf.Abs(CurrentInput.x) > JoystickWalkThreshold) GoalSpeed = Mathf.Sign(CurrentInput.x) * WalkSpeed;
                else GoalSpeed = 0;
                NewSpeed = Mathf.MoveTowards(NewSpeed, GoalSpeed, Time.deltaTime * Acceleration);
                break;
            case EMovementStance.InAir:

                break;
        }

        Physics.SetVelocity(new Vector2(NewSpeed, Physics.Velocity.y));

    }
    
    #region jumping mechanics

    public void AttemptJump()
    {
        switch (MovementStance)
        {
            case EMovementStance.Standing:
                Jump();
                return;
            case EMovementStance.InAir:
                if (DoubleJumpsUsed >= MaxDoubleJump)
                {
                    return;
                }
                Jump();
                return;
        }
    }

    public void StopJump()
    {
        
    }

    public void Jump()
    {
        Physics.SetVelocity(new Vector2(Physics.Velocity.x, JumpVelocity));
    }
    #endregion jumping mechanics

    private void SetMovementStance(EMovementStance MovementStance)
    {
        if (MovementStance == this.MovementStance) return;
        EndMovementStance(this.MovementStance);
        this.MovementStance = MovementStance;
        switch (MovementStance)
        {
            case EMovementStance.Standing:
                DoubleJumpsUsed = 0;
                break;
            case EMovementStance.InAir:

                break;
        }
        EHAnimatorComponent CharacterAnim = OwningCharacter.Anim;
        CharacterAnim.SetInteger(AnimMovementStance, (int)MovementStance);
    }

    private void EndMovementStance(EMovementStance PreviousMovementStance)
    {
        switch (MovementStance)
        {
            case EMovementStance.Standing:
                return;
            case EMovementStance.InAir:
                return;
        }
    }
}
