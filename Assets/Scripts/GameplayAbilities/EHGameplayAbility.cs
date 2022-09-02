using UnityEngine;

using System.Collections.Generic;

public struct FAbilityClipData
{
    public int AnimationHash;
    public int AnimationFrames;
}

[CreateAssetMenu(fileName = "BasicAbility", menuName = "GameplayAbilities/BasicAbility", order = 1)]
public class EHGameplayAbility : ScriptableObject
{
    protected struct FAbilityClipData
    {
        public int AnimationHash;
        public int AnimationFrames;
    }
    
    public EHActor AbilityOwner { get; private set; }
    protected EHAnimatorComponent OwnerAnimator;
    protected EHMovementComponent OwnerMovementComponent;
    protected EHPhysics2D OwnerPhysicsComponent;
    
    [SerializeField]
    protected List<AnimationClip> AbilityClips;

    protected List<FAbilityClipData> AbilityClipDataList = new List<FAbilityClipData>();

    [SerializeField] 
    protected bool BlockPlayerInput = true;

    [SerializeField]
    protected bool IgnoreGravity = false;
    
    [SerializeField]
    protected bool CancelOnStanceChange = false;

    protected int CurrentFramesActive;
    protected int CurrentAbilityIndex;
    private int TotalFramesActive;
    private float CachedPhysicsScale;

    private bool ShouldEarlyCancelAbility;
    
    [SerializeField]
    private float ExitTime = 1;
    [SerializeField]
    private bool HasExitTime = false;


    public virtual void InitializeAbility(EHActor AbilityOwner)
    {
        if (AbilityOwner != null)
        {
            this.AbilityOwner = AbilityOwner;
            OwnerAnimator = AbilityOwner.GetComponent<EHAnimatorComponent>();
            OwnerMovementComponent = AbilityOwner.GetComponent<EHMovementComponent>();
            OwnerPhysicsComponent = AbilityOwner.GetComponent<EHPhysics2D>();
        }

        CurrentFramesActive = 0;
        TotalFramesActive = 0;
        foreach (AnimationClip AbilityClip in AbilityClips)
        {
            FAbilityClipData AbilityClipData = new FAbilityClipData
            {
                AnimationFrames = Mathf.RoundToInt(AbilityClip.length / EHTime.TimePerFrame),
                AnimationHash = Animator.StringToHash(AbilityClip.name)
            };
            AbilityClipDataList.Add(AbilityClipData);
            TotalFramesActive += AbilityClipData.AnimationFrames;
        }
    }
    
    public virtual void BeginAbility()
    {
        CurrentFramesActive = 0;
        CurrentAbilityIndex = 0;

        FAbilityClipData ClipData = AbilityClipDataList[CurrentAbilityIndex];
        OwnerAnimator.StartAnimationClip(ClipData.AnimationHash);
        ShouldEarlyCancelAbility = false;
        if (OwnerMovementComponent != null)
        {
            OwnerMovementComponent.SetIgnorePlayerInput(true);
            OwnerMovementComponent.OnStanceChangeEvent += OnStanceChange;
        }

        if (IgnoreGravity)
        {
            OwnerPhysicsComponent.SetUseGravity(false);
            OwnerPhysicsComponent.SetVelocity(Vector2.zero);
        }
    }

    public virtual void TickAbility()
    {
        ++CurrentFramesActive;
    }

    public virtual void OnAbilityEnd()
    {
        OwnerAnimator.ResetAnimatorState();
        if (OwnerMovementComponent != null)
        {
            OwnerMovementComponent.SetIgnorePlayerInput(false);
            OwnerMovementComponent.OnStanceChangeEvent -= OnStanceChange;
            
            // Return speed to max speed after ability has ended
            if (OwnerPhysicsComponent)
            {
                
                float MaxSpeed =
                    OwnerMovementComponent.GetMaxSpeedFromMovementStance(OwnerMovementComponent.MovementStance);
                OwnerPhysicsComponent.ClampHorizontalVelocity(MaxSpeed);
            }
        }

        if (IgnoreGravity)
        {
            OwnerPhysicsComponent.SetUseGravity(true);
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
    public virtual void ActivateAbility()
    {
        
    }

    protected virtual void OnStanceChange(EMovementStance MovementStance)
    {
        if (CancelOnStanceChange)
        {
            ShouldEarlyCancelAbility = true;
        }
    }
    
    // Can the new cancel
    public bool CanCancelAbility(EHGameplayAbility NewAbility)
    {
        if (!HasExitTime) return false;
        
        return ((float) CurrentFramesActive / (float) TotalFramesActive) >= ExitTime;
    }
}
