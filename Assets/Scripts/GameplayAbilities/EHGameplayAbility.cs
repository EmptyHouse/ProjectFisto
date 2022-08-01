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
    
    [SerializeField]
    protected AnimationClip AbilityClip;

    private int AbilityClipHash;
    [SerializeField]
    private FAbilityEvent[] AbilityEvents;
    private int CurrentFramesActive;
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
    }

    public virtual bool IsAbilityEnded()
    {
        return CurrentFramesActive >= TotalFramesActive;
    }
}
