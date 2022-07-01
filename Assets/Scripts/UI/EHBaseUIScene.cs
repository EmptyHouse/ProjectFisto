using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHBaseUIScene : MonoBehaviour
{
    public bool IsShown { get; private set; }
    
    public virtual void OnShow()
    {
        IsShown = true;
    }

    public virtual void OnHide()
    {
        IsShown = false;
    }
}
