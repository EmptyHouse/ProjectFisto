using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHPlayerCharacter : EHCharacter
{
    private EHDashComponent DashComponent;
    
    protected override void Awake()
    {
        base.Awake();
        DashComponent = GetComponent<EHDashComponent>();
    }

    #region override methods
    public override void SetUpControllerInput(EHPlayerController Controller)
    {
        Controller.BindEventToButtonInput(EButtonInput.Jump, InputJump, EButtonEventType.Button_Pressed);
        Controller.BindEventToButtonInput(EButtonInput.Jump, InputStopJump, EButtonEventType.Button_Release);
        Controller.BindEventToButtonInput(EButtonInput.Attack, InputAttack, EButtonEventType.Button_Pressed);
        Controller.BindEventToButtonInput(EButtonInput.Bow, InputBowAttack, EButtonEventType.Button_Pressed);
        Controller.BindEventToButtonInput(EButtonInput.Dash, InputDash, EButtonEventType.Button_Pressed);

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
        AttackComponent?.AttackPressed();
        // AttackComponent.AttemptAttack(EAttackType.SimpleAttack);
    }

    private void InputBowAttack()
    {
        AttackComponent?.BowPressed();
    }

    private void InputDash()
    {
        DashComponent?.AttemptDash();
        // AttackComponent.AttemptAttack(EAttackType.Dash);
    }
    
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
