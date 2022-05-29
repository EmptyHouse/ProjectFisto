using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EColliderType
{
    Static,
    Moveable,
    Kinematic,
}

public class EHBoxCollider2D : MonoBehaviour
{
    [SerializeField]
    private EColliderType ColliderType;
    
    #region monobehaviour methods

    protected void FixedUpdate()
    {
        
    }

    #endregion monobehaviour methods
}
