using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHBaseProjectile : EHActor
{
    [SerializeField]
    private bool ShouldRotateProjectile;

    protected override void Awake()
    {
        base.Awake();
        AttackComponent.OnDamagedEnemy += OnProjectileDamageEnemy;
    }

    public void LaunchProjectile(Vector2 LaunchVelocity)
    {
        if (Physics)
        {
            Physics.SetVelocity(LaunchVelocity);
        }
    }

    private void OnProjectileDamageEnemy(EHDamageableComponent OtherDamageComponent)
    {
        Destroy(this.gameObject);
    }
}
