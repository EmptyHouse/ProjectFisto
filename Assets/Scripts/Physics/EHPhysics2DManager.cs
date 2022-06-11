using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class EHPhysics2DManager
{

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
        if (ColliderComponentDictionary.ContainsKey(Collider2D.GetColliderType()))
        {
           // Debug.LogWarning("");
            return;
        }

        ColliderComponentDictionary[Collider2D.GetColliderType()].Remove(Collider2D);
    }
    #endregion

    public void UpdatePhysicsLoop(float DeltaTime)
    {
        
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
