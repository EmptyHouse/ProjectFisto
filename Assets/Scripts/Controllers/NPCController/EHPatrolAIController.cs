using System;
using UnityEngine;

/// <summary>
/// We may want to move these to become ai states rather than single ai controllers
/// </summary>
public class EHPatrolAIController : EHAIController
{
    [SerializeField]
    private Vector2 PatrolMin;
    [SerializeField]
    private Vector2 PatrolMax;
    private Vector2 MinPosition;
    private Vector2 MaxPosition;
    private EHMovementComponent MovementComponent;
    
    #region monobehaviour methods

    protected override void Awake()
    {
        base.Awake();
        MovementComponent = GetComponent<EHMovementComponent>();
    }

    protected void Start()
    {
        Vector2 CurrentActorPosition = GetActorPosition();
        MinPosition = CurrentActorPosition + PatrolMin;
        MaxPosition = CurrentActorPosition + PatrolMax;
        MovementComponent.SetHorizontalInput(1);
    }

    private void Update()
    {
        Vector2 Position = GetActorPosition();
        if (Position.x < MinPosition.x)
        {
            MovementComponent.SetHorizontalInput(1);
        }

        if (Position.x > MaxPosition.x)
        {
            MovementComponent.SetHorizontalInput(-1);
        }
    }
    
    #endregion monobehaviour methods
}
