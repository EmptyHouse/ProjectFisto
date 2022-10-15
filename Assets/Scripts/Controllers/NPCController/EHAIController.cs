using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EAIStatus
{
    Sleep,
    Active,
}

public class EHAIController : EHActorComponent
{
    [SerializeField]
    private EHAIState DefaultAIState;
    private EAIStatus CurrentAIStatus;
    private EHAIState CurrentState;
    
    #region monobehaivour methods

    protected override void Awake()
    {
        base.Awake();
        if (!DefaultAIState)
        {
            Debug.LogWarning(this.name + " does not have a DefaultState assigned...");
            return;
        }
        SetNextState(DefaultAIState);
    }

    private void Update()
    {
        TickStateMachine();
    }
    #endregion monobehaviour methods

    private void TickStateMachine()
    {
        EHAIState NextState = CurrentState?.TickState();
        if (NextState != null)
        {
            SetNextState(NextState);
        }
    }

    private void SetNextState(EHAIState NextState)
    {
        if (NextState == CurrentState)
        {
            return;
        }
        
        CurrentState?.EndState();
        CurrentState = NextState;
        CurrentState?.BeginState();
    }

    public void SetCurrentAIState(EAIStatus Status)
    {
        if (this.CurrentAIStatus == Status)
        {
            return;
        }
        this.CurrentAIStatus = Status;
    }
}

public abstract class EHAIState : MonoBehaviour
{
    [SerializeField]
    private string StateId;
    public virtual void BeginState() {}
    public virtual void EndState() {}
    public abstract EHAIState TickState();
}




