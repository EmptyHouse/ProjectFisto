using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHHitstunEffect : EHEffect
{
    public readonly int Anim_Hitstun = Animator.StringToHash("Hitstun");
    private float RemainingHitstunTime;

    protected void Awake()
    {
        EffectType = EEffectType.ActiveEffect;
    }

    public override void OnBeginEffect(EHEffectManagerComponent EffectComponent)
    {
        base.OnBeginEffect(EffectComponent);
        EffectComponent.AssociatedActor.Anim.SetTrigger(Anim_Hitstun);
        EffectComponent.AssociatedActor.Anim.UpdateAnimatorNoTime();
        
        // Remove other hitstun effects from our manager...
        foreach (EHEffect Effect in EffectComponent.GetActiveEffects())
        {
            if (Effect is EHHitstunEffect && Effect != this)
            {
                EffectComponent.RemoveEffect(Effect, true);
            }
        }
    }

    public override void OnEndEffect(EHEffectManagerComponent EffectComponent)
    {
        EffectComponent.AssociatedActor.Anim.ResetAnimatorState();
    }

    public override void TickEffect()
    {
        base.TickEffect();
        RemainingHitstunTime -= EHTime.DeltaTime;
        if (RemainingHitstunTime < 0)
        {
            EffectComponent.RemoveEffect(this);
        }
    }

    public void SetHitstunTime(float HitstunTime)
    {
        RemainingHitstunTime = HitstunTime;
    }
}
