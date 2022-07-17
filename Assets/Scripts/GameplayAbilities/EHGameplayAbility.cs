using UnityEngine;

public class EHGameplayAbility : MonoBehaviour
{
    public EHActor AbilityOwner;
    
    #region monobehaviour methods

    protected void Awake()
    {
        
    }

    #endregion monobehaviour methods
    
    public virtual void BeginAbility()
    {
        
    }

    protected virtual void OnAbilityStarted()
    {
        
    }

    protected virtual void OnAbilityEnd()
    {
        
    }
}
