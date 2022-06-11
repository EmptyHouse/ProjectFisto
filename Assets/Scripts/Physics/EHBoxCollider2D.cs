using System;
using System.Collections;
using System.Collections.Generic;
using EmptyHouseGames.Library;
using UnityEngine;

public enum EColliderType
{
    Static,
    Moveable,
    Kinematic,
    Trigger,
}

public class EHBoxCollider2D : EHActorComponent
{
    #region const variables
    private readonly Vector2 BufferBounds = Vector2.one * 0.02f;
    #endregion const variables
    
    [SerializeField]
    private bool IsTrigger;
    [SerializeField]
    private EColliderType ColliderType;
    [SerializeField, Tooltip("Size of our box collider")]
    private Vector2 BoxSize;
    [SerializeField, Tooltip("The offset position of our box collider")]
    private Vector2 BoxPosition;
    [SerializeField, Tooltip("Determines if this is a character collider. Meaning that it is anchored at the feet rather than centered")]
    private bool IsCharacterCollider;
    
    protected FBox2D CurrentBox;
    protected FBox2D PreviousBox;
    protected FBox2D PhysicsSweepBox;
    
    #region monobehaviour methods

    protected virtual void OnEnable()
    {
        EHGameMode GameMode = GetGameMode<EHGameMode>();
        if (GameMode)
        {
            GameMode.PhysicsManager.AddCollisionComponent(this);
        }
    }

    protected virtual void OnDisable()
    {
        EHGameInstance GameInstance = GetGameInstance();
        if (!GameInstance) return;
        
        EHGameMode GameMode = GetGameMode<EHGameMode>();
        if (GameMode)
        {
            GameMode.PhysicsManager.RemoveCollisionComponent(this);
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying & ColliderType == EColliderType.Kinematic)
        {
            FBox2D.DebugDrawRect(PreviousBox, Color.red);
            FBox2D.DebugDrawRect(PhysicsSweepBox, Color.yellow);
        }
        else if (!Application.isPlaying)
        {
            if (!OwningActor) OwningActor = GetComponent<EHActor>();
            UpdateCurrentBoxGeometry();
        }
        FBox2D.DebugDrawRect(CurrentBox, GetDebugColor());
    }

    #endregion monobehaviour methods

    public EColliderType GetColliderType() => ColliderType;

    public void UpdateKinematicBoxCollider()
    {
        // Update our previous box
        PreviousBox = CurrentBox;
        PreviousBox.Origin += BufferBounds;
        PreviousBox.Size -= (2 * BufferBounds);
        
        UpdateCurrentBoxGeometry();
        
        // Update our Sweep box
        Vector2 MinBounds = new Vector2(Mathf.Min(CurrentBox.MinBounds.x, PreviousBox.MinBounds.x),
            Mathf.Min(CurrentBox.MinBounds.y, PreviousBox.MinBounds.y));
        Vector2 MaxBounds = new Vector2(Mathf.Max(CurrentBox.MaxBounds.x, PreviousBox.MaxBounds.x),
            Mathf.Max(CurrentBox.MaxBounds.y, PreviousBox.MaxBounds.y));
        PhysicsSweepBox = new FBox2D(MinBounds, MaxBounds - MinBounds);
    }
    
    public void UpdateMoveableBoxCollider()
    {
        PreviousBox = CurrentBox;
        
        UpdateCurrentBoxGeometry();
    }
    
    public void UpdateCurrentBoxGeometry()
    {
        Vector2 RectSize = BoxSize * GetActorScale();
        Vector2 RectPosition = GetActorPosition() + BoxPosition - (IsCharacterCollider ? (Vector2.right * RectSize.x / 2f) : (RectSize / 2f));
        CurrentBox.Size = RectSize;
        CurrentBox.Origin = RectPosition;
    }
    
    #region debug functions
    public Color GetDebugColor()
    {
        if (IsTrigger)
        {
            return new Color(.914f, .961f, .256f);
        }
        switch (ColliderType)
        {
            case EColliderType.Static:
                return Color.green;
            case EColliderType.Moveable:
                return new Color(.258f, .96f, .761f);
            case EColliderType.Kinematic:
                return new Color(.761f, .256f, .96f);
            case EColliderType.Trigger:
                return new Color(.914f, .961f, .256f); 
        }
        return Color.green;
    }
    #endregion debug functions
}
