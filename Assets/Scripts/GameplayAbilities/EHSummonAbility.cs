using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SummonAbility", menuName = "GameplayAbilities/SummonAbility", order = 1)]
public class EHSummonAbility : EHGameplayAbility
{
    [SerializeField]
    private EHSummonActor SummonedSpiritPrefab;

    private EHSummonActor SummonedSpirit;

    public override void InitializeAbility(EHActor AbilityOwner)
    {
        base.InitializeAbility(AbilityOwner);
        
        // Create a new summon spirit
        SummonedSpirit = AbilityOwner.SpawnActor(SummonedSpiritPrefab, Vector2.zero);
        SummonedSpirit.InitializeSummon();
        SummonedSpirit.SetActorActive(false);
    }

    public override void BeginAbility()
    {
        base.BeginAbility();
        SummonedSpirit.StartSummonAbility(AbilityOwner);
    }
}
