using System.Collections;
using System.Collections.Generic;
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

public class EHAttackComponent : EHActorComponent
{
    #region const values
    private readonly int Anim_Attack1 = Animator.StringToHash("Attack");
    #endregion const values

    private EHAnimatorComponent Anim;
    [SerializeField]
    private FAttackData DefaultAttackData;
    #region monobehaviour methods

    protected override void Awake()
    {
        base.Awake();
        Anim = OwningActor.Anim;
    }
    #endregion monobehaviour methods

    public void AttemptAttack(int AttackValue)
    {
        Anim.SetTrigger(Anim_Attack1);
    }

    public void AttackDamageComponent(EHDamageableComponent OtherDamageComponent)
    {
        OtherDamageComponent.TakeDamage(DefaultAttackData);
    }
}
