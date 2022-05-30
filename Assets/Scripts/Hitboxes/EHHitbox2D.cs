using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHHitbox2D : MonoBehaviour
{
    #region enums

    public enum EHitboxType
    {
        Hitbox,
        Hurtbox,
    }
    #endregion enums

    [SerializeField] 
    private EHitboxType HitboxType = EHitboxType.Hitbox;

    private HashSet<EHHitbox2D> IntersectingHitboxSet = new HashSet<EHHitbox2D>();
    private EHAttackComponent AttackComponent;
    private EHDamageableComponent DamageComponent;

    protected void Awake()
    {
        
    }
    
}
