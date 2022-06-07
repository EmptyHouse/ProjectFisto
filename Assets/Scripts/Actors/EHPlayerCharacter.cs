using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHPlayerCharacter : EHCharacter
{


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
        
    }

    private void InputMoveVertical(float Value)
    {
        
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
