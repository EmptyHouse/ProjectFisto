using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHPlayerAttackComponent : EHAttackComponent
{
    private bool IsChargePressed;
    
    public void OnBeginChargeAttack()
    {
        IsChargePressed = true;
    }

    public void OnReleaseChargeAttack()
    {
        IsChargePressed = false;
    }
    
    #region attack coroutines
    
    #endregion
}
