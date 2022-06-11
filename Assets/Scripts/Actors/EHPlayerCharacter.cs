using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHPlayerCharacter : EHCharacter
{
    private EHMovementComponent MovementComponent;

    protected override void Awake()
    {
        base.Awake();
        MovementComponent = GetComponent<EHMovementComponent>();
    }

    #region override methods
    public override void SetUpControllerInput(EHPlayerController Controller)
    {
        Controller.BindEventToButtonInput("Jump", InputJump, EButtonEventType.Button_Pressed);
        Controller.BindEventToButtonInput("Jump", InputStopJump, EButtonEventType.Button_Release);
        Controller.BindEventToAxisInput("Horizontal", InputMoveHorizontal);
        Controller.BindEventToAxisInput("Vertical", InputMoveVertical);
    }
    #endregion
    
    #region input functions

    private void InputMoveHorizontal(float Value)
    {
        MovementComponent.SetHorizontalInput(Value);
    }

    private void InputMoveVertical(float Value)
    {
        MovementComponent.SetVerticalInput(Value);
    }

    private void InputJump()
    {
        
    }

    private void InputStopJump()
    {
        
    }

    private void InputAttack()
    {
        
    }

    #endregion input functions
}
