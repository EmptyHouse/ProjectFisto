using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BasicAbility", menuName = "GameplayAbilities/BasicAbility", order = 1)]
public class EHGameplayAbility : ScriptableObject
{
    [System.Serializable]
    protected struct FAbilityEvent
    {
        [Tooltip("The frame that this event will be triggered")]
        public int EventFrame;
        [Tooltip("Event that will be performed at frame")]
        public UnityEvent AbilityEvent;
    }
    public EHActor AbilityOwner { get; private set; }
    protected EHAnimatorComponent OwnerAnimator;
    protected EHMovementComponent OwnerMovementComponent;
    
    [SerializeField]
    protected AnimationClip AbilityClip;

    protected int AbilityClipHash;
    [SerializeField]
    private FAbilityEvent[] AbilityEvents;
    protected int CurrentFramesActive;
    [SerializeField, HideInInspector]
    private int TotalFramesActive;

    #region monobehaviour methods

    #endregion monobehaviour methods
    

    public virtual void InitializeAbility(EHActor AbilityOwner)
    {
        if (AbilityOwner != null)
        {
            this.AbilityOwner = AbilityOwner;
            OwnerAnimator = AbilityOwner.GetComponent<EHAnimatorComponent>();
            OwnerMovementComponent = AbilityOwner.GetComponent<EHMovementComponent>();
        }

        if (CurrentFramesActive >= TotalFramesActive)
        {
            CurrentFramesActive = 0;
            TotalFramesActive = Mathf.RoundToInt(AbilityClip.length / EHTime.TimePerFrame);
        }

        AbilityClipHash = Animator.StringToHash(AbilityClip.name);
    }
    
    public virtual void BeginAbility()
    {
        CurrentFramesActive = 0;
        OwnerAnimator.StartAnimationClip(AbilityClipHash);
        if (OwnerMovementComponent != null)
        {
            OwnerMovementComponent.IgnorePlayerInput = true;
        }
    }

    public virtual void TickAbility()
    {
        ++CurrentFramesActive;
        foreach (FAbilityEvent AbilityEvent in AbilityEvents)
        {
            if (AbilityEvent.EventFrame == CurrentFramesActive)
            {
                AbilityEvent.AbilityEvent.Invoke();
            }
        }
    }

    public virtual void OnAbilityEnd()
    {
        OwnerAnimator.ResetAnimatorState();
        if (OwnerMovementComponent != null)
        {
            OwnerMovementComponent.IgnorePlayerInput = false;
        }
    }

    public virtual bool IsAbilityEnded()
    {
        return CurrentFramesActive >= TotalFramesActive;
    }
    
    // Input
    public virtual void ActivateAbility(bool IsActive)
    {
        
    }
}
