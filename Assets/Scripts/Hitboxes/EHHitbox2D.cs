using System;
using System.Collections;
using System.Collections.Generic;
using EmptyHouseGames.Library;
using UnityEngine;

public enum EHitboxType
{
    Hitbox,
    Hurtbox,
}

public class EHHitbox2D : MonoBehaviour
{
    #region enums

    
    #endregion enums

    [SerializeField] 
    private EHitboxType HitboxType = EHitboxType.Hitbox;

    [SerializeField] 
    private Vector2 HitboxSize = Vector2.one;

    [SerializeField] 
    private Vector2 HitboxOffset = Vector2.zero;
    

    private HashSet<EHHitbox2D> IntersectingHitboxSet = new HashSet<EHHitbox2D>();
    private EHAttackComponent AttackComponent;
    private EHDamageableComponent DamageComponent;
    private EHHitboxComponent HitboxComponent;
    private FBox2D CurrentBox;
    
    protected void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            UpdateHitbox();
        }
        FBox2D.DebugDrawRect(CurrentBox, GetDebugColor(), true);
    }

    private void UpdateHitbox()
    {
        Vector2 TransformScale = transform.localScale;
        Vector2 TransformPosition = transform.position;
        
        Vector2 AdjustedSize = HitboxSize * TransformScale;
        Vector2 AdjustedPosition = TransformPosition + (HitboxOffset - AdjustedSize / 2);
        
        CurrentBox.Origin = AdjustedPosition;
        CurrentBox.Size = AdjustedSize;
    }

    public bool IsHitboxOverlapping(EHHitbox2D OtherHitbox)
    {
        return OtherHitbox.CurrentBox.IsOverlappingBox2D(CurrentBox);
    }
    
    #region debug methods

    private Color GetDebugColor()
    {
        switch (HitboxType)
        {
            case EHitboxType.Hitbox:
                return Color.red; 
            case EHitboxType.Hurtbox:
                return Color.cyan;
        }
        return Color.black;
    }
    #endregion debug methods
}
