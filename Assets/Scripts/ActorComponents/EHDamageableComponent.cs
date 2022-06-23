using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EHDamageableComponent : EHActorComponent
{
    #region delegates
    public UnityAction OnCharacterDiedDel;
    public UnityAction OnCharacterTakeDamage;
    #endregion delegates
    
    [SerializeField] private int MaxHealth;

    private int CurrentHealth;

    protected override void Awake()
    {
        base.Awake();
        CurrentHealth = MaxHealth;
    }


    public int GetMaxHealth() => MaxHealth;
    public int GetHealth() => CurrentHealth;
    public bool GetIsDead() => CurrentHealth <= 0;

    public int TakeDamage(FAttackData AttackData)
    {
        CurrentHealth -= AttackData.DamageApplied;
        OnCharacterTakeDamage?.Invoke();
        if (CurrentHealth <= 0)
        {
            OnCharacterDied();
        }
        return CurrentHealth;
    }

    private void OnCharacterDied()
    {
        OnCharacterDiedDel?.Invoke();
        Destroy(this.gameObject);
    }

    private void DestroyAfterTime()
    {
    }
}
