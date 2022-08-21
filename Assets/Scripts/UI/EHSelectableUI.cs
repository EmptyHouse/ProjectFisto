using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHSelectableUI : MonoBehaviour
{
    public bool IsActive { get; private set; }
    
    #region monobehaivour methods
    
    #endregion monobehaviour methods

    public void SetActive(bool Value)
    {
        if (IsActive == Value) return;
        IsActive = Value;
        enabled = Value;
    }

    protected virtual void OnUIEnabled()
    {
        
    }

    protected virtual void OnUIDisabled()
    {
        
    }
}

