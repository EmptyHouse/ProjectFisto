using System;
using System.Collections;
using System.Collections.Generic;
using EmptyHouseGames.Library;
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
    private bool IsTrigger;
    [SerializeField]
    private EColliderType ColliderType;
    [SerializeField, Tooltip("Size of our box collider")]
    private Vector2 BoxSize;
    [SerializeField, Tooltip("The offset position of our box collider")]
    private Vector2 BoxPosition;
    [SerializeField, Tooltip("Determines if this is a character collider. Meaning that it is anchored at the feet rather than centered")]
    private bool IsCharacterCollider;
    
    protected FBox2D CurrentBox;
    protected FBox2D PreviousBox;
    protected FBox2D PhysicsSweepBox;
    
    #region monobehaviour methods

    private void OnDrawGizmos()
    {
        if (Application.isPlaying & ColliderType == EColliderType.Kinematic)
        {
            FBox2D.DebugDrawRect(PreviousBox, Color.red);
            FBox2D.DebugDrawRect(PhysicsSweepBox, Color.yellow);
        }
        FBox2D.DebugDrawRect(CurrentBox, GetDebugColor());
    }

    #endregion monobehaviour methods
    public void UpdateBoxCollider()
    {
        
    }
    
    #region debug functions
    public Color GetDebugColor()
    {
        if (IsTrigger)
        {
            return new Color(.914f, .961f, .256f);
        }
        switch (ColliderType)
        {
            case EColliderType.Static:
                return Color.green;
            case EColliderType.Moveable:
                return new Color(.258f, .96f, .761f);
            case EColliderType.Kinematic:
                return new Color(.761f, .256f, .96f);
        }
        return Color.green;
    }
    #endregion debug functions
}
