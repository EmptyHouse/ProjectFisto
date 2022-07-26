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
    private readonly int Anim_Attack2 = Animator.StringToHash("Attack2");
    private readonly int Anim_ChargeAttack = Animator.StringToHash("ChargeAttack");
    #endregion const values

    private EHAnimatorComponent Anim;
    [SerializeField] 
    private FAttackData DefaultAttackData;
    private EHPhysics2D PhysicsComponent;
    #region monobehaviour methods

    protected override void Awake()
    {
        base.Awake();
        Anim = OwningActor.Anim;
        PhysicsComponent = GetComponent<EHPhysics2D>();
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

    public void AttemptChargeAttack()
    {
        Anim.SetTrigger(Anim_Attack2);
        Anim.SetBool(Anim_ChargeAttack, true);   
    }

    public void ReleaseChargeAttack()
    {
        Anim.SetBool(Anim_ChargeAttack, false);
    }

    private float ChargeTime = 0;
    
    public void OnBeginChargeAttack()
    {
        StartCoroutine(BeginChargeAttack());
    }
    
    private IEnumerator BeginChargeAttack()
    {
        ChargeTime = 0;
        while (ChargeTime < MaxChargeTime)
        {
            yield return null;
            ChargeTime += EHTime.DeltaTime;
        }

        ChargeTime = Mathf.Min(MaxChargeTime, ChargeTime);
        ReleaseChargeAttack();
    }

    [SerializeField] private float ChargeReleaseSpeed = 5;
    private const float MaxChargeTime = 1.5f;
    public void OnChargeAttackReleased()
    {
        Vector2 ActorScale = GetActorScale();
        PhysicsComponent.SetVelocity(new Vector2(Mathf.Sign(ActorScale.x) * ChargeReleaseSpeed * ChargeTime / MaxChargeTime, 0));
    }
}
