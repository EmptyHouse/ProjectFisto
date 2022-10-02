using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class EHEffectManagerComponent : EHActorComponent
{
    private HashSet<EHEffect> PassiveEffects = new HashSet<EHEffect>();
    private HashSet<EHEffect> ActiveEffects = new HashSet<EHEffect>();

    private void Update()
    {
        TickActiveEffects();
    }

    public void ApplyEffect(EHEffect Effect)
    {
        bool WasAdded = false;
        if (Effect.GetEffectType() == EEffectType.ActiveEffect)
        {
            WasAdded = ActiveEffects.Add(Effect);
        }
        else
        {
            WasAdded = PassiveEffects.Add(Effect);
        }

        if (WasAdded)
        {
            Effect.OnBeginEffect(this);
        }
        else Debug.LogWarning("Effect was not added...");

        if (ActiveEffects.Count > 0) enabled = true;
    }

    public void RemoveEffect(EHEffect Effect, bool IgnoreEndEffect = false)
    {
        bool WasRemoved = false;
        if (Effect.GetEffectType() == EEffectType.ActiveEffect)
        {
            WasRemoved = ActiveEffects.Remove(Effect);
        }
        else
        {
            WasRemoved = PassiveEffects.Remove(Effect);
        }

        if (WasRemoved)
        {
            if (!IgnoreEndEffect) Effect.OnEndEffect(this);
        }
        else Debug.LogWarning("Effect was not ...");

        if (ActiveEffects.Count == 0) enabled = false;

    }

    public void TickActiveEffects()
    {
        foreach (EHEffect Effect in GetActiveEffects())
        {
            Effect.TickEffect();
            if (Effect.ReadyToRemove)
            {
                RemoveEffect(Effect);
            }
        }
    }

    public List<EHEffect> GetActiveEffects() => ActiveEffects.ToList();
}
