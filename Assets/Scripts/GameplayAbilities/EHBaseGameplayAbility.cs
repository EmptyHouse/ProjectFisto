using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EHBaseGameplayAbility : ScriptableObject
{
    #region structs

    public struct FAbilityData
    {
        public float AbilityDuration;
        public int AbilityAnimationHash;

    }
    #endregion structs

    public EHActor AbilityOwner { get; private set; }
    
    
    private AnimationClip AbilityAnimation;
    private float TimePassed;
    
    [SerializeField]
    private FAttackData HitData;

    private FAbilityData AbilityData;

    public virtual void InitializeAbility(EHActor AbilityOwner)
    {
        this.AbilityOwner = AbilityOwner;
        AbilityData.AbilityDuration += AbilityAnimation.length;
        AbilityData.AbilityAnimationHash = Animator.StringToHash(AbilityAnimation.name);
    }

    public virtual bool IsAbilityComplete()
    {
        return TimePassed > AbilityData.AbilityDuration;
    }

    public virtual void UpdateAbility(float DeltaSeconds)
    {
        TimePassed += DeltaSeconds;
    }

    public virtual void BeginAbility()
    {
        TimePassed = 0;
        if (AbilityOwner != null)
        {
            if (AbilityOwner.Anim != null)
            {
                AbilityOwner.Anim.StartAnimationClip(AbilityData.AbilityAnimationHash);
            }
        }
    }

    public virtual void OnAbilityEnd()
    {
        
    }
}
