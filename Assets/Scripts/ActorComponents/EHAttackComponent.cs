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

public class EHAttackComponent : EHCharacterComponent
{
    private EHAnimatorComponent Anim;
    [SerializeField] 
    private FAttackData DefaultAttackData;
    [SerializeField]
    private List<EHGameplayAbility> EquippedAbilities = new List<EHGameplayAbility>();
    [SerializeField]
    private float ChargeReleaseSpeed = 10f;

    [SerializeField]
    private float CrystalKnockSpeed = 10f;
    

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

    public void ReleaseAttack(EAttackType AttackType)
    {
        if (ActiveAbility != null)
        {
            ActiveAbility.ActivateAbility(false);
        }
    }

    public void AttackDamageComponent(EHDamageableComponent OtherDamageComponent)
    {
        OtherDamageComponent.TakeDamage(DefaultAttackData);
    }
    
    #region animation events
    public void OnBeginChargeAttack()
    {
        if (ActiveAbility == null) return;
        EHChargeAbility ChargeAbility = (EHChargeAbility) ActiveAbility;
        float ChargePercent = ChargeAbility.GetChargePercent();
        
        Vector2 ActorScale = GetActorScale();
        EHPhysics2D PhysicsComponent = OwningCharacter.Physics;
        PhysicsComponent.SetVelocity(new Vector2(Mathf.Sign(ActorScale.x) * ChargeReleaseSpeed * ChargePercent, 0));
    }

    public void OnCrystalAttack()
    {
        if (ActiveAbility == null) return;
        OwningCharacter.Physics.SetVelocity(new Vector2(-Mathf.Sign(GetActorScale().x) * CrystalKnockSpeed, 0));
    }
    #endregion animation events
}
