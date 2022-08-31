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
    Dash,
}

public class EHAttackComponent : EHActorComponent
{
    private EHAnimatorComponent Anim;
    [SerializeField] 
    private FAttackData DefaultAttackData;
    [SerializeField]
    private List<EHGameplayAbility> EquippedAbilities = new List<EHGameplayAbility>();
    [SerializeField] 
    private List<EHGameplayAbility> EquippedAbilitiesAir = new List<EHGameplayAbility>();
    [SerializeField]
    private float ChargeReleaseSpeed = 10f;


    private EHGameplayAbility ActiveAbility;
    private EHMovementComponent OwnerMovementComponent;
    
    #region monobehaviour methods
    protected override void Awake()
    {
        base.Awake();
        Anim = AssociatedActor.Anim;
        OwnerMovementComponent = AssociatedActor.GetComponent<EHMovementComponent>();
        for (int i = 0; i < EquippedAbilities.Count; ++i)
        {
            EquippedAbilities[i] = Instantiate(EquippedAbilities[i]);
            EquippedAbilities[i].InitializeAbility(AssociatedActor);
        }

        for (int i = 0; i < EquippedAbilitiesAir.Count; ++i)
        {
            EquippedAbilitiesAir[i] = Instantiate(EquippedAbilities[i]);
            EquippedAbilitiesAir[i].InitializeAbility(AssociatedActor);
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
        if (OwnerMovementComponent != null && EquippedAbilities.Count > AbilityIndex && ActiveAbility == null)
        {
            if (OwnerMovementComponent != null)
            {
                ActiveAbility = OwnerMovementComponent.IsInAir() ? EquippedAbilitiesAir[AbilityIndex] : EquippedAbilities[AbilityIndex];
            }

            if (ActiveAbility != null)
            {
                ActiveAbility.BeginAbility();
            }
        }
    }

    public void ReleaseAttack(EAttackType AttackType)
    {
        // if (ActiveAbility != null)
        // {
        //     ActiveAbility.ActivateAbility();
        // }
    }

    public void AttackDamageComponent(EHDamageableComponent OtherDamageComponent)
    {
        EHActor OtherAssociatedActor = OtherDamageComponent.AssociatedActor;
        if (OtherAssociatedActor == GetOwningActor())
        {
            return;//do not apply damage to owner
        }
        OtherDamageComponent.TakeDamage(DefaultAttackData);
    }
}
