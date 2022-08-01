using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct FAttackData
{
    [HideInInspector]
    // The actor component applying the hit data
    public EHAttackComponent Owner;
    // The raw damage applied to the damage component
    public int DamageApplied;
    // Force direction that will be applied to the damaged component
    public Vector2 DamageForce;
}

public enum EAttackType
{
    SimpleAttack,
    ChargeAttack,
    CrystalAttack,
}

public class EHAttackComponent : EHActorComponent
{
    private EHAnimatorComponent Anim;
    [SerializeField] 
    private FAttackData DefaultAttackData;
    [SerializeField]
    private List<EHGameplayAbility> EquippedAbilities = new List<EHGameplayAbility>();

    private EHGameplayAbility ActiveAbility;
    
    #region monobehaviour methods
    protected override void Awake()
    {
        base.Awake();
        Anim = OwningActor.Anim;
        foreach (EHGameplayAbility Ability in EquippedAbilities)
        {
            Ability.InitializeAbility(OwningActor);
        }
    }

    protected virtual void Update()
    {
        if (ActiveAbility == null) return;
        ActiveAbility.TickAbility();
        
        if (!ActiveAbility.IsAbilityEnded()) return;
        ActiveAbility.OnAbilityEnd();
        ActiveAbility = null;
    }

    #endregion monobehaviour methods

    public void AttemptAttack(EAttackType AttackType)
    {
        int AbilityIndex = (int) AttackType;
        if (EquippedAbilities.Count > AbilityIndex && ActiveAbility == null)
        {
            ActiveAbility = EquippedAbilities[AbilityIndex];
            ActiveAbility.BeginAbility();
        }
    }

    public void AttackDamageComponent(EHDamageableComponent OtherDamageComponent)
    {
        OtherDamageComponent.TakeDamage(DefaultAttackData);
    }
}
