using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHPlayerState : MonoBehaviour
{
    public EHCharacter AssociatedPlayerCharacter { get; private set; }
    public EHPlayerController AssociatedPlayerController { get; private set; }

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
    }
}
