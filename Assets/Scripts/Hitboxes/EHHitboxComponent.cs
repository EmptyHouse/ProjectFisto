using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EHHitboxComponent : EHActorComponent
{
    private Dictionary<EHitboxType, HashSet<EHHitbox2D>> HitboxDictionary = new Dictionary<EHitboxType, HashSet<EHHitbox2D>>()
    {
        { EHitboxType.Hitbox, new HashSet<EHHitbox2D>() },
        { EHitboxType.Hurtbox, new HashSet<EHHitbox2D>() },
    };
    private EHDamageableComponent CachedDamageComponent;
    private EHAttackComponent CachedAttackComponent;
    private HashSet<EHDamageableComponent> IntersectedDamageableComponents = new HashSet<EHDamageableComponent>();

    protected override void Awake()
    {
        base.Awake();
        EHGameInstance.Instance.GameMode.PhysicsManager.AddHitboxComponent(this);
        CachedAttackComponent = GetComponent<EHAttackComponent>();
        CachedDamageComponent = GetComponent<EHDamageableComponent>();
    }

    protected void OnEnable()
    {
        if (EHGameInstance.Instance)
        {
            EHGameInstance.Instance.GameMode.PhysicsManager.AddHitboxComponent(this);
        }
    }

    protected void OnDisable()
    {
        if (EHGameInstance.Instance)
        {
            EHGameInstance.Instance.GameMode.PhysicsManager.RemoveHitboxComponent(this);
        }
    }

    public void CheckHitOtherHitboxComponent(EHHitboxComponent OtherHitboxComponent)
    {
        foreach (EHHitbox2D Hitbox in HitboxDictionary[EHitboxType.Hitbox])
        {
            foreach (EHHitbox2D OtherHurtbox in OtherHitboxComponent.HitboxDictionary[EHitboxType.Hurtbox])
            {
                if (!IntersectedDamageableComponents.Contains(OtherHitboxComponent.CachedDamageComponent) && 
                    Hitbox.IsHitboxOverlapping(OtherHurtbox))
                {
                    CachedAttackComponent.AttackDamageComponent(OtherHitboxComponent.CachedDamageComponent);
                    IntersectedDamageableComponents.Add(OtherHitboxComponent.CachedDamageComponent);
                    return;
                }
            }
        }
    }

    public void UpdateAllHitboxes()
    {
        foreach (KeyValuePair<EHitboxType, HashSet<EHHitbox2D>> HitboxKeyValue in HitboxDictionary)
        {
            foreach (EHHitbox2D Hitbox in HitboxKeyValue.Value)
            {
                Hitbox.UpdateHitbox();
            }
        }
    }

    public HashSet<EHHitbox2D> GetAllHitboxesOfType(EHitboxType HitboxType)
    {
        if (HitboxDictionary.ContainsKey(HitboxType))
        {
            return HitboxDictionary[HitboxType];
        }

        return new HashSet<EHHitbox2D>();
    }

    public void AddActiveHitbox(EHHitbox2D Hitbox)
    {
        if (Hitbox == null) return;
        EHitboxType HitboxType = Hitbox.GetHitboxType();
        if (!HitboxDictionary.ContainsKey(HitboxType))
        {
            HitboxDictionary.Add(HitboxType, new HashSet<EHHitbox2D>());
        }

        HitboxDictionary[HitboxType].Add(Hitbox);
        Hitbox.UpdateHitbox();
    }

    public void RemoveActiveHitbox(EHHitbox2D Hitbox)
    {
        if (Hitbox == null) return;
        EHitboxType HitboxType = Hitbox.GetHitboxType();
        if (!HitboxDictionary.ContainsKey(HitboxType))
        {
            HitboxDictionary.Add(HitboxType, new HashSet<EHHitbox2D>());
        }

        HitboxDictionary[HitboxType].Remove(Hitbox);
        
        if (HitboxDictionary.ContainsKey(EHitboxType.Hitbox) && 
            HitboxDictionary[EHitboxType.Hitbox].Count <= 0)
        {
            // Reset all intersected damage components so we can hit them again
            IntersectedDamageableComponents.Clear();
        }
    }
}
