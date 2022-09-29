using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EHProjectileComponent : EHActorComponent
{
    [SerializeField]
    private Transform LaunchPivot;
    [SerializeField]
    private EHBaseProjectile Projectile;
    [SerializeField] 
    private float LaunchSpeed = 15;

    public void LaunchProjectile()
    {
        EHBaseProjectile NewProjectile = AssociatedActor.SpawnActor(Projectile, LaunchPivot.position, 0);
        Vector2 LaunchVelocity = LaunchPivot.right * LaunchSpeed;
        LaunchVelocity.x *= Mathf.Sign(GetActorScale().x);
        NewProjectile.LaunchProjectile(LaunchVelocity);
    }
}
