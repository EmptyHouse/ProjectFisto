using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class AIPatrolForPlayer : EHAIState
{
    [SerializeField]
    private Vector2 LeftPosition;
    [SerializeField]
    private Vector2 RightPosition;
    [SerializeField]
    private float VisionRange;
    
    [SerializeField]
    private EHAIState ChasePlayerState;

    private Vector2 OriginPosition;
    private Vector2 GoalPosition;
    private bool MovingLeft = true;
    
    #region monobehaivour methods

    

    private void Start()
    {
        OriginPosition = transform.position;
        UpdateGoalPosition(MovingLeft);
    }

    #endregion monobehaivour methods

    public override void BeginState(EHAIController Controller)
    {
        base.BeginState(Controller);
        UpdateGoalPosition(true);
    }

    public override EHAIState TickState()
    {
        if (IsPlayerInRange())
        {
            return ChasePlayerState;
        }

        float Offset = GoalPosition.x - GetAICharacter().GetPosition().x;
        if (MovingLeft && Offset > 0)
        {
            UpdateGoalPosition(false);
        }
        else if (!MovingLeft && Offset < 0)
        {
            UpdateGoalPosition(true);
        }
        return null;
    }

    private bool IsPlayerInRange()
    {
        EHCharacter Character = GetAICharacter();
        if (Character == null) 
            return false;
        float LookingScale = Mathf.Sign(Character.GetScale().x);
        Vector2 VisionPosition = Character.GetPosition();
        Vector2 PlayerPosition = GetPlayerCharacter().GetPosition();
        Vector2 Offset = PlayerPosition - VisionPosition;
        
        return Mathf.Sign(Offset.x) == Mathf.Sign(LookingScale) && Offset.sqrMagnitude < (VisionRange * VisionRange);
    }

    private void UpdateGoalPosition(bool IsMovingLeft)
    {
        this.MovingLeft = IsMovingLeft;
        GoalPosition = OriginPosition + (MovingLeft ? LeftPosition : RightPosition);
        EHCharacter Character = GetAICharacter();
        Character.MovementComponent.SetHorizontalInput(IsMovingLeft ? -1 : 1);
        Character.MovementComponent.ResetDirection();
    }
}
