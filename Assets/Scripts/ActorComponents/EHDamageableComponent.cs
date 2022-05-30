using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EHDamageableComponent : EHActorComponent
{
    #region delegates

    public UnityAction OnCharacterDiedDel;
    #endregion delegates
    
    [SerializeField] private int MaxHealth;

    [SerializeField] private int CurrentHealth;


    public int GetMaxHealth() => MaxHealth;
    public int GetHealth() => CurrentHealth;
    public bool GetIsDead() => CurrentHealth <= 0;

    public int TakeDamage(FAttackData AttackData)
    {
        CurrentHealth -= AttackData.DamageApplied;
        if (CurrentHealth <= 0)
        {
            OnCharacterDied();
        }

        return CurrentHealth;
    }

    private void OnCharacterDied()
    {
        OnCharacterDiedDel?.Invoke();
    }
}
