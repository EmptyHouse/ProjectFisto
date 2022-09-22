using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboAbility", menuName = "GameplayAbilities/ComboAbility", order = 1)]
public class EHComboAttackAbility : EHBaseGameplayAbility
{
    // while playing this clip, the player early out into the next attack
    public AnimationClip TransitionClip;
    public EHComboAttackAbility NextAttackAbility;
    private FAbilityData TransitionAbilityData;
    private float TotalComboAttackTime;

    public override void InitializeAbility(EHActor AbilityOwner)
    {
        base.InitializeAbility(AbilityOwner);
        TransitionAbilityData.AbilityDuration = TransitionClip.length;
        TransitionAbilityData.AbilityAnimationHash = Animator.StringToHash(TransitionClip.name);
        TotalComboAttackTime = AbilityData.AbilityDuration + TransitionAbilityData.AbilityDuration;
    }

    public override bool IsAbilityComplete()
    {
        return TimePassed > TotalComboAttackTime || ShouldEarlyOut;
    }

    public override void OnInputPressed()
    {
        
    }
}
