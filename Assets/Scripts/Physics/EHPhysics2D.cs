using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHPhysics2D : EHActorComponent
{
    #region const variables

    public const float GravityConstant = 9.8f;

    #endregion const variables
    
    public Vector2 Velocity { 
        get => velocity;
        private set => velocity = value;
    }
    private Vector2 velocity = Vector2.zero;
    
    public Vector2 GravityVector { get; private set; } = Vector2.down;
    public Vector2 GravityRight => new Vector2(-GravityVector.y, GravityVector.x);
    public Vector2 GravityLeft => new Vector2(GravityVector.y, -GravityVector.x);
    public Vector2 GravityUp => -GravityVector;
    public Vector2 GravityDown => GravityVector;
    
    [SerializeField]
    private bool UseGravity = true;
    
    [SerializeField]
    private bool UseTerminalVelocity = true;

    [SerializeField] 
    public float GravityScale = 1f;
    
    
    #region monobehaviour methods
    protected virtual void OnEnable()
    {
        EHGameMode GameMode = GetGameMode<EHGameMode>();
        if (GameMode)
        {
            GameMode.PhysicsManager.AddPhysicsComponent(this);
        }
    }

    protected void OnDisable()
    {
        EHGameInstance GameInstance = GetGameInstance();
        if (!GameInstance) return;
        
        EHGameMode GameMode = GetGameMode<EHGameMode>();
        if (GameMode)
        {
            GameMode.PhysicsManager.RemovePhysicsComponent(this);
        }
    }
    #endregion monobehaviour methods
    
    #region update methods

    public void UpdateVelocityFromGravity(float DeltaTime)
    {
        Velocity += Vector2.down * GravityConstant * DeltaTime * GravityScale;
    }

    public void UpdatePositionBasedOnVelocity(float DeltaTime)
    {
        SetActorPosition(GetActorPosition() + Velocity * DeltaTime);
    }
    #endregion update methods
    
    
    #region getter/setter methods
    public void SetVelocity(Vector2 Velocity)
    {
        this.Velocity = Velocity;
    }

    public void ClampHorizontalVelocity(float MaxVelocity)
    {
        if (Velocity.x > MaxVelocity)
        {
            Velocity = new Vector2(MaxVelocity, Velocity.y);
        }
        else if (Velocity.x < -MaxVelocity)
        {
            Velocity = new Vector2(-MaxVelocity, Velocity.y);
        }
    }

    public void SetUseGravity(bool UseGravity)
    {
        this.UseGravity = UseGravity;
    }

    public void SetUseTerminalVelocity(bool UseTerminalVelocity)
    {
        this.UseTerminalVelocity = UseTerminalVelocity;
    }

    public void SetGravityVector(Vector2 GravityVector)
    {
        this.GravityVector = GravityVector;
    }
    #endregion getter/setter methods

    public void OnCollisionEvent(Vector2 CollisionDirection)
    {
        if (CollisionDirection.y != 0) velocity.y = 0;
        if (CollisionDirection.x != 0) velocity.x = 0;

    }
}
