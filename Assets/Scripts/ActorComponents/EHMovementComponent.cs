using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

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
    private const float JoystickDeadzone = .15f;
    // Input Minimum before we register a walking movement
    private const float JoystickWalkThreshold = JoystickDeadzone;
    // Input minimum before we register a run movement
    private const float JoystickRunThreshold = .65f;
    private const float JoystickCrouchThreshold = -0.6f;

    private readonly int Anim_HorizontalInput = Animator.StringToHash("HInput");
    private readonly int Anim_VerticalInput = Animator.StringToHash("VInput");
    private readonly int Anim_VerticalVelocity = Animator.StringToHash("VVelocity");

    private readonly int Anim_MovementStance = Animator.StringToHash("MovementStance");
    private readonly int Anim_StanceChange = Animator.StringToHash("StanceChange");
    
    #endregion const variables
    [SerializeField]
    private bool IgnorePlayerInput;
    
    [SerializeField]
    private bool IsRight = true;
    [SerializeField] 
    private float GroundAcceleration = 50f;
    [SerializeField] 
    private float AirAcceleration = 10f;
    
    [SerializeField]
    private float WalkSpeed = 5f;
    [SerializeField]
    private float RunSpeed = 15f;
    [SerializeField] 
    private float MaxAirSpeed = 15f;
    
    [Header("Jumping Values")]
    [SerializeField]
    private int MaxDoubleJump = 1;
    [SerializeField]
    private float TimeToReachApex = 1;
    [SerializeField]
    private float JumpHeightApex = 1.5f;
    [HideInInspector, SerializeField] 
    private float JumpVelocity;

    public EMovementStance MovementStance { get; private set; } = EMovementStance.Standing;
    private EHBoxCollider2D ColliderComponent;
    private EHPhysics2D Physics;
    private int DoubleJumpsUsed;
    
    // Input values
    private Vector2 CurrentInput;
    private Vector2 PreviousInput;
    public UnityAction<EMovementStance> OnStanceChangeEvent;
    
    

    #region monobehaviour methods
    protected override void Awake()
    {
        base.Awake();
        Physics = GetComponent<EHPhysics2D>();
        ColliderComponent = GetComponent<EHBoxCollider2D>();
        ColliderComponent.OnStartCollision += OnCollisionBegin;
    }

    private void OnCollisionBegin(FCollisionData CollisionData)
    {
        if (CollisionData.Direction.y > 0)
        {
            if (IsInAir())
            {
                SetMovementStance(EMovementStance.Standing);
            }
        }
    }

    private void OnValidate()
    {
        if (TimeToReachApex != 0)
        {
            if (Physics == null) Physics = GetComponent<EHPhysics2D>();
            
            Physics.GravityScale = (2 * JumpHeightApex) / (EHPhysics2D.GravityConstant * Mathf.Pow(TimeToReachApex, 2));
            JumpVelocity = 2 * JumpHeightApex / TimeToReachApex;
        }

        if (Application.isPlaying) return;
        
        if (AssociatedActor == null) InitializeOwningActor();
        
        SetIsRight(IsRight, true);
    }

    private void Update()
    {
        if (MovementStance != EMovementStance.InAir && Mathf.Abs(Physics.Velocity.y) > 0)
        {
            SetMovementStance(EMovementStance.InAir);
        }

        UpdateMovementFromInput();
        
        PreviousInput = CurrentInput;
    }

    #endregion monobehaviour methods

    public float GetMaxSpeedFromMovementStance(EMovementStance MovementStance)
    {
        switch (MovementStance)
        {
            case EMovementStance.Standing:
                return RunSpeed;
            case EMovementStance.InAir:
                return MaxAirSpeed;
        }

        return RunSpeed;
    }
    
    public void SetHorizontalInput(float Input)
    {
        if (Mathf.Abs(Input) < JoystickDeadzone) 
            Input = 0;
        
        CurrentInput.x = Input;
        AssociatedActor.Anim.SetFloat(Anim_HorizontalInput, Mathf.Abs(Input));
        
        if (Input != 0) SetIsRight(Input > 0);
    }

    public void SetVerticalInput(float Input)
    {
        if (Mathf.Abs(Input) < JoystickDeadzone) 
            Input = 0;
        
        CurrentInput.y = Input;
        AssociatedActor.Anim.SetFloat(Anim_VerticalInput, Input);
    }

    private void UpdateMovementFromInput()
    {
        float GoalSpeed = 0;
        float Acceleration = 0;
        float NewSpeed = Physics.Velocity.x;
        float xInput = IgnorePlayerInput ? 0 :CurrentInput.x;
        
        switch (MovementStance)
        {
            case EMovementStance.Standing:
                Acceleration = GroundAcceleration;
                if (Mathf.Abs(xInput) > JoystickRunThreshold) GoalSpeed = Mathf.Sign(xInput) * RunSpeed;
                else if (Mathf.Abs(xInput) > JoystickWalkThreshold) GoalSpeed = Mathf.Sign(xInput) * WalkSpeed;
                else GoalSpeed = 0;
                break;
            case EMovementStance.InAir:
                AssociatedActor.Anim.SetFloat(Anim_VerticalVelocity, Physics.Velocity.y);
                Acceleration = AirAcceleration;
                if (xInput > 0f) GoalSpeed = MaxAirSpeed;
                else if (xInput < 0f) GoalSpeed = -MaxAirSpeed;
                else GoalSpeed = NewSpeed;
                break;
        }

        NewSpeed = Mathf.MoveTowards(NewSpeed, GoalSpeed, EHTime.DeltaTime * Acceleration);
        Physics.SetVelocity(new Vector2(NewSpeed, Physics.Velocity.y));
    }

    #region jumping mechanics

    public void AttemptJump()
    {
        if (IgnorePlayerInput) return;
        
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
                ++DoubleJumpsUsed;
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
        if (CurrentInput.x != 0f)
        {
            SetIsRight(CurrentInput.x > 0, true);
        }
    }
    #endregion jumping mechanics

    private void SetIsRight(bool IsRight, bool ForceDirection = false)
    {
        if (IgnorePlayerInput && !ForceDirection) return;
        
        if (this.IsRight == IsRight) return;
        
        this.IsRight = IsRight;
        Vector2 ActorScale = GetActorScale();
        ActorScale.x = (IsRight ? 1 : -1) * Mathf.Abs(ActorScale.x);
        SetActorScale(ActorScale);
    }

    private void SetMovementStance(EMovementStance MovementStance)
    {
        if (MovementStance == this.MovementStance) return;
        EndMovementStance(this.MovementStance);
        this.MovementStance = MovementStance;
        switch (MovementStance)
        {
            case EMovementStance.Standing:
                DoubleJumpsUsed = 0;
                if (Mathf.Abs(CurrentInput.x) > 0)
                    SetIsRight(CurrentInput.x > 0, true);
                break;
            case EMovementStance.InAir:
                
                break;
        }
        EHAnimatorComponent CharacterAnim = OwningCharacter.Anim;
        CharacterAnim.SetTrigger(Anim_StanceChange);
        CharacterAnim.SetInteger(Anim_MovementStance, (int)MovementStance);
        OnStanceChangeEvent?.Invoke(MovementStance);
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
    
    public void SetIgnorePlayerInput(bool IgnorePlayerInput)
    {
        this.IgnorePlayerInput = IgnorePlayerInput;
        if (IgnorePlayerInput) return;
        SetHorizontalInput(CurrentInput.x);
    }

    public bool IsInAir()
    {
        return MovementStance == EMovementStance.InAir;
    }

    public void ResetDirection()
    {
        if (Mathf.Abs(CurrentInput.x) < JoystickDeadzone)
        {
            return;
        }
        
        SetIsRight(CurrentInput.x > 0, true);
    }
}
