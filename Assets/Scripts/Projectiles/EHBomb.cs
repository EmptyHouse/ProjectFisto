using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHBomb : EHBaseProjectile
{
    private readonly int Anim_Explode = Animator.StringToHash("Explode");
    
    private bool IsExploding = false;
    
    protected override void Awake()
    {
        base.Awake();
        AttackComponent.OnDamagedEnemy += OnHitEnemy;
        ColliderComponent.OnStartCollision += OnBeginOverlap;
    }

    private void OnHitEnemy(EHDamageableComponent DamageableComponent)
    {
        OnExplode();
    }

    private void OnBeginOverlap(FCollisionData OverlappingCollider)
    {
        OnExplode();
    }

    public void OnExplode()
    {
        if (IsExploding) return;
        IsExploding = true;
        ColliderComponent.enabled = false;
        Physics.enabled = false;
        Anim.SetTrigger(Anim_Explode);
    }

    protected override void OnProjectileDamageEnemy(EHDamageableComponent OtherDamageComponent)
    {
    }

    public void OnExplosionComplete()
    {
        Destroy(this.gameObject);
    }
}
