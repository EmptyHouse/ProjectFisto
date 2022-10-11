using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class EHPhysics2DManager
{
    private HashSet<EHHitboxComponent> HitboxComponentSet = new HashSet<EHHitboxComponent>();
    private HashSet<EHPhysics2D> PhysicsSet = new HashSet<EHPhysics2D>();
    private Dictionary<EColliderType, HashSet<EHBoxCollider2D>> ColliderComponentDictionary =
        new Dictionary<EColliderType, HashSet<EHBoxCollider2D>>();
    private HashSet<EHBoxCollider2D> TriggerColliderSet = new HashSet<EHBoxCollider2D>();

    public EHPhysics2DManager()
    {
        ColliderComponentDictionary.Add(EColliderType.Static, new HashSet<EHBoxCollider2D>());
        ColliderComponentDictionary.Add(EColliderType.Moveable, new HashSet<EHBoxCollider2D>());
        ColliderComponentDictionary.Add(EColliderType.Kinematic, new HashSet<EHBoxCollider2D>());
        ColliderComponentDictionary.Add(EColliderType.Trigger, new HashSet<EHBoxCollider2D>());
        
        foreach (EHBoxCollider2D collider in GameObject.FindObjectsOfType<EHBoxCollider2D>()) AddCollisionComponent(collider);
    }
    
    #region adding/removing physics components

    public void AddPhysicsComponent(EHPhysics2D PhysicsComponent)
    {
        if (PhysicsComponent == null)
        {
            Debug.LogWarning("Physics Component was null");
            return;
        }
        if (!PhysicsSet.Add(PhysicsComponent))
        {
            //Debug.LogWarning("Physics Component already added: " + PhysicsComponent.name);
        }
    }

    public void RemovePhysicsComponent(EHPhysics2D PhysicsComponent)
    {
        if (PhysicsComponent == null)
        {
            Debug.LogWarning("Physics Component was null");
            return;
        }
        if (!PhysicsSet.Remove(PhysicsComponent))
        {
            //Debug.LogWarning("Physics component was not found: " + PhysicsComponent.name);
        }
    }

    public void AddCollisionComponent(EHBoxCollider2D Collider2D)
    {
        if (!ColliderComponentDictionary.ContainsKey(Collider2D.GetColliderType()))
        {
            //Debug.LogWarning("Collider Type not found");
            return;
        }

        ColliderComponentDictionary[Collider2D.GetColliderType()].Add(Collider2D);
    }

    public void RemoveCollisionComponent(EHBoxCollider2D Collider2D)
    {
        if (!ColliderComponentDictionary.ContainsKey(Collider2D.GetColliderType()))
        {
           // Debug.LogWarning("");
            return;
        }

        ColliderComponentDictionary[Collider2D.GetColliderType()].Remove(Collider2D);
    }

    public void AddHitboxComponent(EHHitboxComponent HitboxComponent)
    {
        if (HitboxComponent == null) return;

        HitboxComponentSet.Add(HitboxComponent);
    }

    public void RemoveHitboxComponent(EHHitboxComponent HitboxComponent)
    {
        if (HitboxComponent == null) return;
        
        HitboxComponentSet.Remove(HitboxComponent);
    }
    #endregion

    public void UpdatePhysicsLoop(float DeltaTime)
    {
        UpdateHitboxes();
        
        foreach (EHPhysics2D Rigid in PhysicsSet)
        {
            Rigid.UpdateVelocityFromGravity(DeltaTime);
            Rigid.UpdatePositionBasedOnVelocity(DeltaTime);
        }

        foreach (EHBoxCollider2D MoveableCollider in ColliderComponentDictionary[EColliderType.Moveable])
        {
            MoveableCollider.UpdateMoveableBoxCollider();
        }
        foreach (EHBoxCollider2D PhysicsCollider in ColliderComponentDictionary[EColliderType.Kinematic])
        {
            PhysicsCollider.UpdateKinematicBoxCollider();
        }
        UpdateKinematicColliders();
    }

    private void UpdateKinematicColliders()
    {
        foreach (EHBoxCollider2D PhysicsCollider in ColliderComponentDictionary[EColliderType.Kinematic])
        {
            foreach (EHBoxCollider2D StaticCollider in ColliderComponentDictionary[EColliderType.Static])
            {
                if (!Physics2D.GetIgnoreLayerCollision(PhysicsCollider.gameObject.layer, StaticCollider.gameObject.layer) 
                    && StaticCollider.CheckPhysicsColliderOverlapping(PhysicsCollider))
                {
                    if (StaticCollider.PushOutCollider(PhysicsCollider, out Vector2 PushDirection))
                    {
                        PhysicsCollider.TranslateActorPosition(PushDirection);
                        if (PhysicsCollider.PhysicsComponent)
                        {
                            PhysicsCollider.PhysicsComponent.OnCollisionEvent(PushDirection);
                        }
                        PhysicsCollider.UpdateCurrentBoxGeometry();
                        PhysicsCollider.AddOverlappingCollision(StaticCollider, PushDirection);
                    }
                }
                else
                {
                    PhysicsCollider.RemoveOverlappingCollision(StaticCollider);
                }
            }

            foreach (EHBoxCollider2D TriggerCollider in ColliderComponentDictionary[EColliderType.Trigger])
            {
                if (!Physics2D.GetIgnoreLayerCollision(PhysicsCollider.gameObject.layer,
                        TriggerCollider.gameObject.layer)
                    && TriggerCollider.CheckPhysicsColliderOverlapping(PhysicsCollider))
                {
                    TriggerCollider.AddOverlappingCollision(PhysicsCollider, Vector2.zero);
                }
                else
                {
                    TriggerCollider.RemoveOverlappingCollision(PhysicsCollider);
                }
            }
        }
    }

    private void UpdateHitboxes()
    {
        foreach (EHHitboxComponent HitboxComponent in HitboxComponentSet)
        {
            HitboxComponent.UpdateAllHitboxes();
        }

        List<EHHitboxComponent> HitboxComponentList = HitboxComponentSet.ToList();
        for (int i = 0; i < HitboxComponentList.Count - 1; ++i)
        {
            for (int j = i + 1; j < HitboxComponentList.Count; ++j)
            {
                HitboxComponentList[i].CheckHitOtherHitboxComponent(HitboxComponentList[j]);
                HitboxComponentList[j].CheckHitOtherHitboxComponent(HitboxComponentList[i]);
            }
        }
    }

    private struct CollisionNode : System.IComparable
    {
        public float CollisionDistance;
        public EHBoxCollider2D Collider;

        public CollisionNode(float CollisionDistance, EHBoxCollider2D Collider)
        {
            this.CollisionDistance = CollisionDistance;
            this.Collider = Collider;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 0;
            CollisionNode OtherNode = (CollisionNode) obj;
            return (int)Mathf.Sign(CollisionDistance - OtherNode.CollisionDistance);
        }
    }
}
