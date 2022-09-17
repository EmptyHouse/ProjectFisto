using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EHSummonActor : EHActor
{
    [System.Serializable]
    private struct FSummonAttackData
    {
        public AnimationClip AttackAnimationClip;
        public FAttackData AttackData;
    }
    
    [SerializeField]
    private AnimationClip AnimClip;
    private int TotalTimePassed;
    private float TimeToRefreshAbility = 0;

    public void InitializeSummon(EHActor AbilityOwner)
    {
       SetOwner(AbilityOwner);
    }

    public void StartSummonAbility()
    {
        
    }

    public void OnSummonAbilityEnd()
    {
        SetActorActive(false);
    }

    private IEnumerator FadeOutSummonActor()
    {
        float TimeToFade = 0.35f;
        float TimePassed = 0;
        
        while (TimePassed < TimeToFade)
        {
            yield return null;
        }
    }
}
