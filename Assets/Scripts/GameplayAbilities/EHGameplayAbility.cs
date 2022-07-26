using UnityEngine;

public class EHGameplayAbility
{
    public EHActor AbilityOwner;

    public virtual void BeginAbility(EHActor AbilityOwner)
    {
        
    }

    protected virtual void TickAbility()
    {
        
    }

    protected virtual void OnAbilityEnd()
    {
        
    }
}
