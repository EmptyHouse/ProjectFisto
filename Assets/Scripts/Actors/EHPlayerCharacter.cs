using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHPlayerCharacter : EHCharacter
{
    #region override methods
    public override void SetUpControllerInput(EHPlayerController Controller)
    {
        Controller.BindEventToButtonInput(EButtonInput.Jump, InputJump, EButtonEventType.Button_Pressed);
        Controller.BindEventToButtonInput(EButtonInput.Jump, InputStopJump, EButtonEventType.Button_Release);
        Controller.BindEventToButtonInput(EButtonInput.Attack, InputAttack, EButtonEventType.Button_Pressed);
        Controller.BindEventToButtonInput(EButtonInput.ChargeAttack, InputChargeAttack, EButtonEventType.Button_Pressed);
        Controller.BindEventToButtonInput(EButtonInput.ChargeAttack, InputChargeAttackRelease, EButtonEventType.Button_Release);

        Controller.BindEventToAxisInput(EAxisInput.Horizontal, InputMoveHorizontal);
        Controller.BindEventToAxisInput(EAxisInput.Vertical, InputMoveVertical);
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

    private void InputChargeAttack()
    {
        AttackComponent.AttemptChargeAttack();
    }

    private void InputChargeAttackRelease()
    {
        AttackComponent.ReleaseChargeAttack();
    }

    #endregion input functions
}
