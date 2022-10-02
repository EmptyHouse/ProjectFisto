using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEffectType
{
    PassiveEffect, // Effects that do not update while they are applied
    ActiveEffect, // Effects that update while they are applied
}

public class EHEffect : ScriptableObject
{
    [SerializeField]
    protected EEffectType EffectType;

    protected EHEffectManagerComponent EffectComponent;
    
    public bool ReadyToRemove { get; protected set; }

    public virtual void OnBeginEffect(EHEffectManagerComponent EffectComponent)
    {
        this.EffectComponent = EffectComponent;
    }

    public virtual void OnEndEffect(EHEffectManagerComponent EffectComponent)
    {
        
    }

    public virtual void TickEffect()
    {
        
    }

    public EEffectType GetEffectType() => EffectType;
}


