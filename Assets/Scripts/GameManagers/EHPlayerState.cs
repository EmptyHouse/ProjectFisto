using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHPlayerState : MonoBehaviour
{
    public EHCharacter AssociatedPlayerCharacter { get; private set; }
    public EHPlayerController AssociatedPlayerController { get; private set; }
    public EHPlayerInventory PlayerInventory { get; private set; }

    protected virtual void Awake()
    {
        PlayerInventory = GetComponent<EHPlayerInventory>();
    }

    public void PossessPlayerCharacter(EHCharacter PlayerCharacter)
    {
        this.AssociatedPlayerCharacter = PlayerCharacter;
    }
    
    public void PossessPlayerController(EHPlayerController PlayerController)
    {
        AssociatedPlayerController = PlayerController;
        if (!AssociatedPlayerController) return;
        
        if (AssociatedPlayerCharacter)
        {
            AssociatedPlayerCharacter.SetUpControllerInput(AssociatedPlayerController);
        }
        SetupPlayerControllerActions(PlayerController);
    }

    private void SetupPlayerControllerActions(EHPlayerController PlayerController)
    {
        PlayerController.BindEventToButtonInput(EButtonInput.Crystal, InputInventoryAction, EButtonEventType.Button_Pressed);
    }
    
    #region inventory functions

    public void InputInventoryAction()
    {
        EHInventoryItem Item = PlayerInventory.GetCurrentActiveItem();
        if (Item == null) return;
        Item.StartItemAbility();
    }
    #endregion inventory functions
}
