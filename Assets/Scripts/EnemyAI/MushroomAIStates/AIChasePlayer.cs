using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChasePlayer : EHAIState
{
    [SerializeField]
    private EHAIState PatrolForPlayerState;
    [SerializeField]
    private float VisionRange;
    [SerializeField]
    private float AttackRange;
    [SerializeField]
    private float AttackCoolDown;
    
    public override EHAIState TickState()
    {
        if (IsPlayerOutOfRange())
        {
            return PatrolForPlayerState;
        }

        EHCharacter Character = GetAICharacter();
        if (IsPlayerInAttackRange())
        {
            Character.AttackComponent?.AttackPressed();
        }

        EHCharacter PlayerCharacter = GetPlayerCharacter();
        Character.MovementComponent?.SetHorizontalInput(Mathf.Sign(PlayerCharacter.GetPosition().x - Character.GetPosition().x));
        return null;
    }

    private bool IsPlayerOutOfRange()
    {
        Vector2 PlayerPosition = GetPlayerCharacter().GetPosition();
        Vector2 Position = GetAICharacter().GetPosition();
        
        return (PlayerPosition - Position).sqrMagnitude > (VisionRange * VisionRange);
    }

    private bool IsPlayerInAttackRange()
    {
        Vector2 PlayerPosition = GetPlayerCharacter().GetPosition();
        Vector2 Position = GetAICharacter().GetPosition();

        if (Mathf.Abs(PlayerPosition.x - Position.x) < AttackRange)
        {
            return true;
        }

        return false;
    }
}
