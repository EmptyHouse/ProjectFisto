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

    [SerializeField] 
    protected bool BlockPlayerInput;
    
    [SerializeField]
    protected bool CancelOnStanceChange;

    protected int AbilityClipHash;
    [SerializeField]
    private FAbilityEvent[] AbilityEvents;
    protected int CurrentFramesActive;
    [SerializeField, HideInInspector]
    private int TotalFramesActive;

    private bool ShouldEarlyCancelAbility;

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
        ShouldEarlyCancelAbility = false;
        if (OwnerMovementComponent != null)
        {
            OwnerMovementComponent.SetIgnorePlayerInput(true);
            OwnerMovementComponent.OnStanceChangeEvent += OnStanceChange;
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
            OwnerMovementComponent.SetIgnorePlayerInput(false);
            OwnerMovementComponent.OnStanceChangeEvent -= OnStanceChange;
        }
    }

    public virtual bool IsAbilityEnded()
    {
        return CurrentFramesActive >= TotalFramesActive || ShouldEarlyCancelAbility;
    }
    
    /// <summary>
    /// User Input to activate an ability
    /// NOTE: May want to change the name of this function
    /// </summary>
    /// <param name="IsActive"></param>
    public virtual void ActivateAbility(bool IsActive)
    {
        
    }

    protected virtual void OnStanceChange(EMovementStance MovementStance)
    {
        if (CancelOnStanceChange)
        {
            ShouldEarlyCancelAbility = true;
        }
    }
}
