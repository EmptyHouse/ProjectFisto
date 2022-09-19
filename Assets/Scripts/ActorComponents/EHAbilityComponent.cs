using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHAbilityComponent : EHActorComponent
{
    [SerializeField, Tooltip("Abilities that our character will start with")]
    private List<EHBaseGameplayAbility> InitialAbilities;
    
    private List<EHBaseGameplayAbility> EquippedAbilities;
    private EHBaseGameplayAbility CurrentActiveAbility;
    
    #region monobeahviour methods

    protected override void Awake()
    {
        base.Awake();
        EquippedAbilities = new List<EHBaseGameplayAbility>();
        foreach (EHBaseGameplayAbility GameplayAbility in InitialAbilities)
        {
            AddAbility(GameplayAbility);
        }
    }

    protected virtual void Update()
    {
        if (!CurrentActiveAbility)
        {
            return;
        }
        CurrentActiveAbility.TickAbility(EHTime.DeltaTime);
        if (CurrentActiveAbility.IsAbilityComplete())
        {
            CurrentActiveAbility.OnAbilityEnd();
            CurrentActiveAbility = null;
        }
    }

    #endregion monobehaviour methods
    
    #region manage abilities

    /// <summary>
    /// Begins an ability. If no parameter is passed in, it will start the first ability listed
    /// </summary>
    /// <param name="AbilityIndex"></param>
    public void StartAbility(int AbilityIndex = 0)
    {
        if (AbilityIndex < 0 || AbilityIndex >= EquippedAbilities.Count)
        {
            Debug.LogWarning("Invalid index: " + AbilityIndex);
            return;
        }

        if (CurrentActiveAbility != null) return;
        CurrentActiveAbility = EquippedAbilities[0];
        CurrentActiveAbility.BeginAbility();
    }
    #endregion manage abilities
    
    #region add/remove abilities
    public void AddAbility(EHBaseGameplayAbility AbilityToAdd)
    {
        EHBaseGameplayAbility NewAbility = Instantiate(AbilityToAdd);
        NewAbility.InitializeAbility(AssociatedActor);
        EquippedAbilities.Add(NewAbility);
    }

    public void RemoveEquippedAbility(EHBaseGameplayAbility AbilityToRemove)
    {
        if (AbilityToRemove == null)
        {
            Debug.LogWarning("Ability that was passed in was null. Cannot create...");
            return;
        }
        if (!EquippedAbilities.Contains(AbilityToRemove))
        {
            Debug.LogWarning("Ability was not found. Perhaps it was already removed?");
            return;
        }

        EquippedAbilities.Remove(AbilityToRemove);
    }

    public void ClearEquippedAbilities()
    {
        foreach (EHBaseGameplayAbility Ability in EquippedAbilities)
        {
            RemoveEquippedAbility(Ability);
        }
    }
    #endregion add/remove abilities
}
