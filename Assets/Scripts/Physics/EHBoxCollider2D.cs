using System;
using System.Collections;
using System.Collections.Generic;
using EmptyHouseGames.Library;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public enum EColliderType
{
    Static,
    Moveable,
    Kinematic,
    Trigger,
}

public struct FCollisionData
{
    public EHBoxCollider2D Collider;
    public Vector2 Direction;
}

public class EHBoxCollider2D : EHActorComponent
{
    public UnityAction<FCollisionData> OnStartCollision;
    public UnityAction<EHBoxCollider2D> OnOverlapBegin;
    public UnityAction<EHBoxCollider2D> OnOverlapEnd;
    private HashSet<EHBoxCollider2D> OverlappingCollisions;
    #region const variables
    private readonly Vector2 BufferBounds = Vector2.one * 0.02f;
    #endregion const variables
    
    [SerializeField]
    private bool IsTrigger = false;
    [SerializeField]
    protected EColliderType ColliderType = EColliderType.Static;
    [SerializeField, Tooltip("Size of our box collider")]
    private Vector2 BoxSize = Vector2.one;
    [SerializeField, Tooltip("The offset position of our box collider")]
    private Vector2 BoxPosition = Vector2.zero;
    [SerializeField, Tooltip("Determines if this is a character collider. Meaning that it is anchored at the feet rather than centered")]
    private bool IsCharacterCollider = false;

    protected FBox2D CurrentBox;
    protected FBox2D PreviousBox;
    protected FBox2D PhysicsSweepBox;
    public EHPhysics2D PhysicsComponent { get; private set; }
    
    #region monobehaviour methods

    protected override void Awake()
    {
        base.Awake();
        UpdateCurrentBoxGeometry();
        PreviousBox = CurrentBox;
        if (ColliderType == EColliderType.Kinematic)
        {
            PhysicsComponent = GetComponent<EHPhysics2D>();
            OverlappingCollisions = new HashSet<EHBoxCollider2D>();
        }
        else if (ColliderType == EColliderType.Trigger)
        {
            OverlappingCollisions = new HashSet<EHBoxCollider2D>();
        }
    }

    protected void OnDestroy()
    {
        EHGameInstance GameInstance = GetGameInstance();
        if (!GameInstance) return;
        
        EHGameMode GameMode = GetGameMode<EHGameMode>();
        if (GameMode)
        {
            GameMode.PhysicsManager.RemoveCollisionComponent(this);
        }
    }

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

    protected virtual void OnDrawGizmos()
    {
        if (Application.isPlaying & ColliderType == EColliderType.Kinematic)
        {
            FBox2D.DebugDrawRect(PreviousBox, Color.red);
            FBox2D.DebugDrawRect(PhysicsSweepBox, Color.yellow);
        }
        else if (!Application.isPlaying)
        {
            if (!AssociatedActor) AssociatedActor = GetComponent<EHActor>();
            UpdateCurrentBoxGeometry();
        }
        FBox2D.DebugDrawRect(CurrentBox, GetDebugColor());
    }

    #endregion monobehaviour methods

    public EColliderType GetColliderType() => ColliderType;
    
    #region update functions
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
    
    public virtual void UpdateCurrentBoxGeometry()
    {
        Vector2 ActorScale = GetActorScale();
        // Convert scale to a positive value
        ActorScale.x = Mathf.Abs(ActorScale.x);
        Vector2 RectSize = BoxSize * ActorScale;
        Vector2 RectPosition = GetActorPosition() + BoxPosition - (IsCharacterCollider ? (Vector2.right * RectSize.x / 2f) : (RectSize / 2f));
        CurrentBox.Size = RectSize;
        CurrentBox.Origin = RectPosition;
    }
    #endregion update functions
    
    #region collision checks

    public virtual bool CheckPhysicsColliderOverlapping(EHBoxCollider2D PhysicsCollider)
    {
        return CurrentBox.IsOverlappingBox2D(PhysicsCollider.PhysicsSweepBox); 
    }

    public bool CheckColliderOverlapping(EHBoxCollider2D OtherCollider)
    {
        return CurrentBox.IsOverlappingBox2D(OtherCollider.CurrentBox);
    }
    #endregion collision checks
    
    #region collision functions

    public virtual bool PushOutCollider(EHBoxCollider2D OtherCollider, out Vector2 PushDirection)
    {
        Vector2 RightUpOffset = CurrentBox.MaxBounds - OtherCollider.CurrentBox.MinBounds;
        Vector2 LeftBottomOffset = CurrentBox.MinBounds - OtherCollider.CurrentBox.MaxBounds;
        PushDirection = Vector2.zero;
        if (PreviousBox.MaxBounds.y < OtherCollider.PreviousBox.MinBounds.y && RightUpOffset.y > 0)
        {
            PushDirection = Vector2.up * RightUpOffset.y;
        }
        else if (PreviousBox.MaxBounds.x < OtherCollider.PreviousBox.MinBounds.x && RightUpOffset.x > 0)
        {
            PushDirection = Vector2.right * RightUpOffset.x;
        }
        else if (PreviousBox.MinBounds.x > OtherCollider.PreviousBox.MaxBounds.x && LeftBottomOffset.x < 0)
        {
            PushDirection = Vector2.right * LeftBottomOffset.x;
        }
        else if (PreviousBox.MinBounds.y > OtherCollider.PreviousBox.MaxBounds.y && LeftBottomOffset.y < 0)
        {
            PushDirection = Vector2.up * LeftBottomOffset.y;
        }
        else return false;
        
        return true;
    }
    #endregion collision functions
    
    #region getter functions

    public FBox2D GetCurrentBoxBounds()
    {
        return CurrentBox;
    }

    public FBox2D GetPreviousBoxBounds()
    {
        return PreviousBox;
    }

    public FBox2D GetPhysicsSweepBounds()
    {
        return PhysicsSweepBox;
    }

    public void AddOverlappingCollision(EHBoxCollider2D OtherBoxCollider, Vector2 Direction)
    {
        if (OverlappingCollisions.Add(OtherBoxCollider))
        {
            FCollisionData CollisionData = new FCollisionData()
            {
                Collider = OtherBoxCollider,
                Direction = Direction,
            };
            if (ColliderType == EColliderType.Trigger)
                OnOverlapBegin?.Invoke(OtherBoxCollider);
            else
                OnStartCollision?.Invoke(CollisionData);
        }
    }

    public void RemoveOverlappingCollision(EHBoxCollider2D OtherBoxCollider)
    {
        if (OverlappingCollisions.Contains(OtherBoxCollider))
        {
            if (OverlappingCollisions.Remove(OtherBoxCollider))
            {
                OnOverlapEnd?.Invoke(OtherBoxCollider);
            }
        }
    }
    #endregion getter functions
    
    #region debug functions
    protected Color GetDebugColor()
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
