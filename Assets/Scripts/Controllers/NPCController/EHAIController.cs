using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EAIStatus
{
    Sleep,
    Active,
}

public class EHAIController : EHCharacterComponent
{
    [SerializeField]
    private EHAIState DefaultAIState;

    public EHPlayerCharacter PlayerCharacter => EHGameInstance.Instance.PlayerCharacter;
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
        SetCurrentAIState(EAIStatus.Active);
    }

    protected void Start()
    {
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
        CurrentState?.BeginState(this);
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
    protected EHAIController AIController;
    
    private void Awake()
    {
        AIController = GetComponentInParent<EHAIController>();
    }

    public virtual void BeginState(EHAIController Controller)
    {
    }
    
    public virtual void EndState() {}
    public abstract EHAIState TickState();
    public EHCharacter GetAICharacter() => AIController?.AssociatedCharacter;
    public EHPlayerCharacter GetPlayerCharacter() => AIController?.PlayerCharacter;
}




