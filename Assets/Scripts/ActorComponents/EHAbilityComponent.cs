using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHAbilityComponent : EHActorComponent
{
    private List<EHBaseGameplayAbility> InitialAbilities;
    private List<EHBaseGameplayAbility> EquippedAbilities;
    
    #region monobeahviour methods

    protected override void Awake()
    {
        base.Awake();
        foreach (EHBaseGameplayAbility GameplayAbility in InitialAbilities)
        {
            
        }
    }

    #endregion monobehaviour methods
}
