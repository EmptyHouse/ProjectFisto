using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHHitboxComponent : EHActorComponent
{
    private Dictionary<EHitboxType, HashSet<EHHitbox2D>> HitboxDictionary = new Dictionary<EHitboxType, HashSet<EHHitbox2D>>();

    protected void Update()
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
    }
}
