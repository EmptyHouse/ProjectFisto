using UnityEngine;
using UnityEngine.Events;

public class EHGameplayAbility : MonoBehaviour
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
    [SerializeField]
    private FAbilityEvent[] AbilityEvents;
    [SerializeField]
    private bool IsLooping = false;
    private int CurrentFramesActive;
    [SerializeField, HideInInspector]
    private int TotalFramesActive;

    #region monobehaviour methods
    private void OnValidate()
    {
        if (Application.isEditor && AbilityClip != null)
        {
            TotalFramesActive = Mathf.RoundToInt(AbilityClip.length / EHTime.TimePerFrame);
        }
    }

    private void Update()
    {
        TickAbility();
    }

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
            if (!IsLooping)
            {
                OnAbilityEnd();
            }
            else
            {
                CurrentFramesActive = 0;
            }
        }
    }
    
    public virtual void BeginAbility()
    {
        CurrentFramesActive = 0;
    }

    protected virtual void TickAbility()
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

    protected virtual void OnAbilityEnd()
    {
        
    }
}
