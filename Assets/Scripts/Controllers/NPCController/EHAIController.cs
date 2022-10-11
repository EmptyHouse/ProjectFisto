using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public enum EAIStatus
{
    Sleep,
    Active,
}

public class EHAIController : EHActorComponent
{
    #region state machine
    [SerializeField]
    private List<EHAIState> AllStates = new List<EHAIState>();
    [SerializeField]
    private EHAIState CurrentAIState;
    [SerializeField]
    private EAIStatus AIStatus;


    public void SetAIStatus(EAIStatus AIStatus, bool ForceUpdate = false)
    {
        if (this.AIStatus == AIStatus && !ForceUpdate)
        {
            return;
        }

        this.AIStatus = AIStatus;
        // 
        switch (AIStatus)
        {
            case EAIStatus.Sleep:
                this.enabled = false;
                break;
            case EAIStatus.Active:
                this.enabled = true;
                break;
        }
    }

    public void SetCurrentAIState(EHAIState AIState)
    {
        CurrentAIState?.EndState();
        CurrentAIState = AIState;
        CurrentAIState.BeginState();
    }
    #endregion state machine
    
    #region monobehaviour methods

    protected override void Awake()
    {
        base.Awake();
        SetAIStatus(AIStatus, true);
        if (AllStates.Count > 0)
        {
            foreach (var State in AllStates)
            {
                State.InitializeState();
            }
            SetCurrentAIState(AllStates[0]);
        }
    }

    protected virtual void Update()
    {
        CurrentAIState?.TickState();
    }

    #endregion monobehaviour methods

}

public class EHAIState : ScriptableObject
{
    protected EHPlayerCharacter PlayerReference;
    
    public virtual void InitializeState()
    {
        PlayerReference = EHGameInstance.Instance.PlayerCharacter;
    }
    
    public virtual void BeginState()
    {
    }

    public virtual void EndState()
    {
        
    }

    public virtual void TickState()
    {
        
    }
}
