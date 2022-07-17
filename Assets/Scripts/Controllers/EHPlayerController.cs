using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public enum EButtonEventType
{
    Button_Release,
    Button_Pressed,
}

[System.Flags]
public enum EButtonInput : byte
{
    Jump = 0x01,
    Attack = 0x02,
    ChargeAttack = 0x04,
}

public enum EAxisInput : byte
{
    Horizontal,
    Vertical,
    Size,
}

public class EHPlayerController : MonoBehaviour
{
    #region const variables
    #endregion const variables
    
    private FButtonInputs Inputs;
    private Dictionary<EButtonInput, UnityAction> ButtonPressedActions = new Dictionary<EButtonInput, UnityAction>();
    private Dictionary<EButtonInput, UnityAction> ButtonReleasedActions = new Dictionary<EButtonInput, UnityAction>();
    private Dictionary<EAxisInput, UnityAction<float>> AxisActions = new Dictionary<EAxisInput, UnityAction<float>>();

    private InputAction MoveAction;
    
    #region monobeahviour methosd

    private void Awake()
    {
        InitializeButtons();
        Inputs.PreviousAxisInput = new float[(int)EAxisInput.Size];
    }

    private void Update()
    {
        Vector2 MoveAxis = MoveAction.ReadValue<Vector2>();
        float[] InputArray = new float[(int)EAxisInput.Size];
        InputArray[(int) EAxisInput.Horizontal] = MoveAxis.x;
        InputArray[(int) EAxisInput.Vertical] = MoveAxis.y;
        Inputs.CurrentAxisInput = InputArray;

        for (int i = 0; i < 3; ++i)//Update this with the number of valid inputs we have
        {
            EButtonInput ButtonInput = (EButtonInput) (1 << i);
            if (Inputs.GetButtonDown(ButtonInput) && ButtonPressedActions.ContainsKey(ButtonInput))
            {
                ButtonPressedActions[ButtonInput]?.Invoke();
            }

            if (Inputs.GetButtonUp(ButtonInput) && ButtonReleasedActions.ContainsKey(ButtonInput))
            {
                ButtonReleasedActions[ButtonInput]?.Invoke();
            }
        }
        Inputs.PreviousInput = Inputs.CurrentInput;

        for (int i = 0; i < (int) EAxisInput.Size; ++i)
        {
            EAxisInput AxisInput = (EAxisInput) i;
            if (Inputs.GetAxisUpdated(AxisInput))
            {
                AxisActions[AxisInput]?.Invoke(Inputs.CurrentAxisInput[(int)AxisInput]);
            }
        }
        Inputs.PreviousAxisInput = Inputs.CurrentAxisInput.ToArray();
    }

    #endregion monobehaviour methods

    private void InitializeButtons()
    {
        PlayerInput InGamePlayerInput = GetComponent<PlayerInput>();
        InputAction JumpAction = InGamePlayerInput.actions["Jump"];
        JumpAction.started += OnJumpAction;
        JumpAction.canceled += OnJumpAction;
        InputAction AttackAction = InGamePlayerInput.actions["Attack1"];
        AttackAction.started += OnAttackAction;
        AttackAction.canceled += OnAttackAction;
        InputAction ChargeAttackAction = InGamePlayerInput.actions["ChargeAttack"];
        ChargeAttackAction.started += OnChargeAttackAction;
        ChargeAttackAction.canceled += OnChargeAttackAction;
        
        MoveAction = InGamePlayerInput.actions["Move"];
    }

    public void BindEventToButtonInput(EButtonInput ButtonId, UnityAction ButtonAction, EButtonEventType EventType)
    {
        Dictionary<EButtonInput, UnityAction> ButtonActionMap;
        switch (EventType)
        {
            case EButtonEventType.Button_Pressed:
                ButtonActionMap = ButtonPressedActions;
                break;
            case EButtonEventType.Button_Release:
                ButtonActionMap = ButtonReleasedActions;
                break;
            default: 
                ButtonActionMap = null;
                break;
        }
        
        if (!ButtonActionMap.ContainsKey(ButtonId))
        {
            ButtonActionMap.Add(ButtonId, null);
        }

        ButtonActionMap[ButtonId] += ButtonAction;
    }

    public void BindEventToAxisInput(EAxisInput AxisId, UnityAction<float> AxisAction)
    {
        if (!AxisActions.ContainsKey(AxisId))
        {
            AxisActions.Add(AxisId, null);
        }

        AxisActions[AxisId] += AxisAction;
    }

    private void UpdateCurrentInput(EButtonInput ButtonInput, bool IsPressed)
    {
        if (IsPressed)
        {
            Inputs.CurrentInput |= ButtonInput;
        }
        else
        {
            Inputs.CurrentInput &= (~ButtonInput);
        }
    }
    #region button events
    private void OnJumpAction(CallbackContext Context) =>
        UpdateCurrentInput(EButtonInput.Jump, Context.ReadValueAsButton());

    private void OnAttackAction(CallbackContext Context) =>
        UpdateCurrentInput(EButtonInput.Attack, Context.ReadValueAsButton());

    private void OnChargeAttackAction(CallbackContext Context) =>
        UpdateCurrentInput(EButtonInput.ChargeAttack, Context.ReadValueAsButton());
    
    #endregion button events

    private struct FButtonInputs
    {
        public EButtonInput PreviousInput;
        public EButtonInput CurrentInput;
        public float[] PreviousAxisInput;
        public float[] CurrentAxisInput;

        public bool GetButtonDown(EButtonInput ButtonInput)
        {
            return (CurrentInput & ButtonInput) == ButtonInput && ((PreviousInput ^ CurrentInput) & ButtonInput) == ButtonInput;
        }

        public bool GetButtonUp(EButtonInput ButtonInput)
        {
            return (PreviousInput & ButtonInput) == ButtonInput && ((PreviousInput ^ CurrentInput) & ButtonInput) == ButtonInput;
        }

        public bool GetButtonHeld(EButtonInput ButtonInput)
        {
            return (CurrentInput & ButtonInput) != 0;
        }

        public float GetAxis(EAxisInput AxisInput)
        {
            return CurrentAxisInput[(int)AxisInput];
        }

        public bool GetAxisUpdated(EAxisInput AxisInput)
        {
            return PreviousAxisInput[(int) AxisInput] != CurrentAxisInput[(int) AxisInput];
        }
    }
}
