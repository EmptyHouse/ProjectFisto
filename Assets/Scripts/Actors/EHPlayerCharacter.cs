using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHPlayerCharacter : EHCharacter
{
    public EHMovementComponent MovementComponent { get; private set; }
    public EHAttackComponent AttackComponent { get; private set; }
    

    protected override void Awake()
    {
        base.Awake();
        MovementComponent = GetComponent<EHMovementComponent>();
        AttackComponent = GetComponent<EHAttackComponent>();
    }

    #region override methods
    public override void SetUpControllerInput(EHPlayerController Controller)
    {
        Controller.BindEventToButtonInput("Jump", InputJump, EButtonEventType.Button_Pressed);
        Controller.BindEventToButtonInput("Jump", InputStopJump, EButtonEventType.Button_Release);
        Controller.BindEventToButtonInput("Attack", InputAttack, EButtonEventType.Button_Pressed);

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
        MovementComponent.AttemptJump();
    }

    private void InputStopJump()
    {
        MovementComponent.StopJump();
    }

    private void InputAttack()
    {
        AttackComponent.AttemptAttack(0);
    }

    #endregion input functions
}
