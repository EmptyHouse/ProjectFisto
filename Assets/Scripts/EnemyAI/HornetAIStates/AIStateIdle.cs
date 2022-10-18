using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateIdle : EHAIState
{
    [SerializeField]
    private float VisionRange;
    [SerializeField]
    private EHAIState AggroState;
    
    public override EHAIState TickState()
    {
        if (IsPlayerInRange())
        {
            return AggroState;
        }
        return null;
    }
    
    // Make this a common function for vision... will be used in multiple states
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
}
