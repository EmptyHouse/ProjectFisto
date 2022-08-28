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
        AttackComponent.AttemptAttack(EAttackType.SimpleAttack);
    }

    // private void InputChargeAttack()
    // {
    //     AttackComponent.AttemptAttack(EAttackType.ChargeAttack);
    // }
    //
    // private void InputChargeAttackRelease()
    // {
    //     AttackComponent.ReleaseAttack(EAttackType.ChargeAttack);
    // }
    //
    // private void InputCrystalAttack()
    // {
    //     AttackComponent.AttemptAttack(EAttackType.CrystalAttack);
    // }
    #endregion input functions
}
