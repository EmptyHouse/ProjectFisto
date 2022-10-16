using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public struct FAttackData
{
    // The raw damage applied to the damage component
    public int DamageApplied;

    public float CameraShakeTime;

    public float CameraShakeIntensity;

    public float HitFreezeTime;

    public float HitstunTime;
    // Force direction that will be applied to the damaged component
    public Vector2 DamageForce;
}

public class EHAttackComponent : EHActorComponent
{
    #region events

    public UnityAction<EHDamageableComponent> OnDamagedEnemy;
    #endregion events
    
    
    private readonly int Anim_AttackTrigger = Animator.StringToHash("Attack");
    private readonly int Anim_BowTrigger = Animator.StringToHash("Bow");
    private const float AttackBufferTime = EHTime.TimePerFrame * 8;
    
    private EHAnimatorComponent Anim;
    [SerializeField] 
    private FAttackData DefaultAttackData;


    private EHMovementComponent OwnerMovementComponent;
    
    #region monobehaviour methods
    protected override void Awake()
    {
        base.Awake();
        Anim = AssociatedActor.GetComponent<EHAnimatorComponent>();
        OwnerMovementComponent = AssociatedActor.GetComponent<EHMovementComponent>();
    }

    #endregion monobehaviour methods

    public void AttackPressed()
    {
        Anim.SetTrigger(Anim_AttackTrigger, AttackBufferTime);
    }

    public void BowPressed()
    {
        Anim.SetTrigger(Anim_BowTrigger, AttackBufferTime);
    }

    public void AttackDamageComponent(EHDamageableComponent OtherDamageComponent)
    {
        EHActor OtherAssociatedActor = OtherDamageComponent.AssociatedActor;
        if (OtherAssociatedActor == GetOwningActor())
        {
            return;//do not apply damage to owner
        }

        EHPhysics2D OtherPhysics = OtherAssociatedActor.GetComponent<EHPhysics2D>();
        
        EHHitstunEffect HitstunEffect = ScriptableObject.CreateInstance<EHHitstunEffect>();
        HitstunEffect.SetHitstunTime(DefaultAttackData.HitstunTime);
        OtherAssociatedActor?.EffectManagerComponent.ApplyEffect(HitstunEffect);
        
        OtherDamageComponent.TakeDamage(DefaultAttackData);
        EHPlayerController PlayerController = GetGameInstance().PlayerController;
        PlayerController.AssociatedCamera.StartCameraShake(DefaultAttackData.CameraShakeTime, DefaultAttackData.CameraShakeIntensity);
        GetGameMode<EHGameMode>().FreezeTime(DefaultAttackData.HitFreezeTime);
        float ScaleDirection = Mathf.Sign(GetActorScale().x);
        OtherPhysics.SetVelocity(new Vector2(ScaleDirection * DefaultAttackData.DamageForce.x, DefaultAttackData.DamageForce.y));
        OnDamagedEnemy?.Invoke(OtherDamageComponent);
    }
    
    #region events

    private void OnAttackStart()
    {
        OwnerMovementComponent.ResetDirection();
    }

    private void OnAttackEnd()
    {
        
    }
    #endregion events
}
