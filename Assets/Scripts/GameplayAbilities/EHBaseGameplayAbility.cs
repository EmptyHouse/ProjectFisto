using UnityEngine;

[CreateAssetMenu(fileName = "BaseAbility", menuName = "GameplayAbilities/BaseAbility", order = 0)]
public class EHBaseGameplayAbility : ScriptableObject
{
    #region structs

    public struct FAbilityData
    {
        public float AbilityDuration;
        public int AbilityAnimationHash;

    }
    #endregion structs

    public EHActor AbilityOwner { get; private set; }
    public EHPhysics2D OwnerPhysicsComponent { get; private set; }
    public EHMovementComponent OwnerMovementComponent { get; private set; }
    
    [SerializeField]
    private AnimationClip AbilityAnimation;
    
    [SerializeField]
    private bool EndOnStanceChange;
    
    protected float TimePassed;
    
    [SerializeField]
    private FAttackData HitData;

    protected bool ShouldEarlyOut;

    protected FAbilityData AbilityData;

    public virtual void InitializeAbility(EHActor AbilityOwner)
    {
        this.AbilityOwner = AbilityOwner;
        OwnerPhysicsComponent = AbilityOwner.GetComponent<EHPhysics2D>();
        OwnerMovementComponent = AbilityOwner.GetComponent<EHMovementComponent>();
        
        AbilityData.AbilityDuration += AbilityAnimation.length;
        AbilityData.AbilityAnimationHash = Animator.StringToHash(AbilityAnimation.name);
        if (EndOnStanceChange && OwnerMovementComponent)
        {
            OwnerMovementComponent.OnStanceChangeEvent += OnStanceChangeEvent;
        }
    }

    public virtual void OnInputPressed()
    {
        
    }

    public virtual void OnInputReleased()
    {
        
    }

    public virtual bool IsAbilityComplete()
    {
        return TimePassed > AbilityData.AbilityDuration || ShouldEarlyOut;
    }

    public virtual void TickAbility(float DeltaSeconds)
    {
        TimePassed += DeltaSeconds;
    }

    public virtual void BeginAbility()
    {
        TimePassed = 0;
        ShouldEarlyOut = false;
        if (AbilityOwner.Anim != null)
        {
            AbilityOwner.Anim.StartAnimationClip(AbilityData.AbilityAnimationHash);
        }
    }

    public virtual void OnAbilityEnd()
    {
        if (AbilityOwner.Anim != null)
        {
            AbilityOwner.Anim.ResetAnimatorState();
        }
    }

    protected virtual void OnStanceChangeEvent(EMovementStance MovementStance)
    {
        ShouldEarlyOut = true;
    }
}
