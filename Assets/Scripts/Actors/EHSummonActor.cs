using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EHSummonActor : EHActor
{
    [SerializeField]
    private AnimationClip AnimClip;
    private FAbilityClipData AbilityClip;
    private int TotalTimePassed;
    private float TimeToRefreshAbility = 0;

    public void InitializeSummon()
    {
        if (AnimClip != null)
        {
            AbilityClip = new FAbilityClipData()
            {
                AnimationFrames = Mathf.RoundToInt(AnimClip.length / EHTime.TimePerFrame),
                AnimationHash = Animator.StringToHash(AnimClip.name)
            };
        }
    }

    protected void Update()
    {
        ++TotalTimePassed;
        if (TotalTimePassed > AbilityClip.AnimationFrames)
        {
            OnSummonAbilityEnd();
        }
    }

    public void StartSummonAbility(EHActor AbilityOwner)
    {
        SetActorActive(true);
        TotalTimePassed = 0;
        Anim.StartAnimationClip(AbilityClip.AnimationHash);
        Vector2 OwnerScale = AbilityOwner.GetScale();
        Vector2 OriginalScale = GetScale();
        SetScale(new Vector2(Mathf.Sign(OwnerScale.x) * Mathf.Abs(OriginalScale.x), OriginalScale.y));
        SetPosition(AbilityOwner.GetPosition());
    }

    public void OnSummonAbilityEnd()
    {
        SetActorActive(false);
    }

    public bool IsAttackReady()
    {
        return false;
    }
}
