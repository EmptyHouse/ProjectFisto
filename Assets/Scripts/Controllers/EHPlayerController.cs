using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum EButtonEventType
{
    Button_Release,
    Button_Pressed,
}

public class EHPlayerController : MonoBehaviour
{
    [System.Serializable]
    private class FButtonInput
    {
        public string ButtonId;
        public KeyCode PrimaryKey;
        public KeyCode AlternateKey;
        public UnityAction OnButtonPressedDel;
        public UnityAction OnButtonReleaseDel;

        public bool IsKeyHeld => Input.GetKey(PrimaryKey) || Input.GetKey(AlternateKey);
        public bool IsKeyPressed => Input.GetKeyDown(PrimaryKey) || Input.GetKeyDown(AlternateKey);
        public bool IsKeyReleased => Input.GetKeyUp(PrimaryKey) || Input.GetKeyUp(AlternateKey);
    }
    
    [System.Serializable]
    private class FAxisInput
    {
        public string AxisId;
        [HideInInspector]
        public float CachedAxisValue;
        
        public float AxisValue => Input.GetAxisRaw(AxisId);

        public UnityAction<float> OnAxisChangedDel;
    }

    [SerializeField]
    private List<FButtonInput> ButtonInputs = new List<FButtonInput>();
    [SerializeField]
    private List<FAxisInput> AxisInputs = new List<FAxisInput>();
    #region monobehaviour methods

    private void Update()
    {
        UpdateButtonEvents();
        UpdateAxisEvents();
    }

    #endregion monobehaivour methods

    public void BindEventToButtonInput(string ButtonId, UnityAction ButtonAction, EButtonEventType EventType)
    {
        FButtonInput ButtonInput = null;
        foreach (FButtonInput ButtonData in ButtonInputs)
        {
            if (ButtonData.ButtonId == ButtonId)
            {
                ButtonInput = ButtonData;
                break;
            }
        }

        if (ButtonInput == null)
        {
            Debug.LogWarning("Invalid ButtonId: " + ButtonId);
            return;
        }
        
        switch (EventType)
        {
            case EButtonEventType.Button_Pressed:
                ButtonInput.OnButtonPressedDel += ButtonAction;
                return;
            case EButtonEventType.Button_Release:
                ButtonInput.OnButtonReleaseDel += ButtonAction;
                return;
        }
    }

    public void BindEventToAxisInput(string AxisId, UnityAction<float> AxisAction)
    {
        FAxisInput AxisInput = null;
        foreach (FAxisInput AxisData in AxisInputs)
        {
            if (AxisData.AxisId == AxisId)
            {
                AxisInput = AxisData;
                break;
            }
        }

        if (AxisInput == null)
        {
            Debug.LogWarning("Axis Id Was Invalid: " + AxisId);
            return;
        }

        AxisInput.OnAxisChangedDel += AxisAction;
    }

    private void UpdateButtonEvents()
    {
        foreach (FButtonInput ButtonInputData in ButtonInputs)
        {
            if (ButtonInputData.IsKeyPressed)
            {
                ButtonInputData.OnButtonPressedDel?.Invoke();
            }

            if (ButtonInputData.IsKeyReleased)
            {
                ButtonInputData.OnButtonReleaseDel?.Invoke();
            }
        }
    }

    private void UpdateAxisEvents()
    {
        foreach (FAxisInput AxisInputData in AxisInputs)
        {
            if (AxisInputData.AxisValue != AxisInputData.CachedAxisValue)
            {
                AxisInputData.CachedAxisValue = AxisInputData.AxisValue;
                AxisInputData.OnAxisChangedDel?.Invoke(AxisInputData.AxisValue);
            }
        }
    }
}
