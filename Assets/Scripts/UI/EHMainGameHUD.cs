using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class EHMainGameHUD : EHBaseGameHUD
{
    
    #region monobevious methods
    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            EHGameInstance.Instance.DebugResetGame();
        }
    }

    #endregion monobehaviour methods
}
