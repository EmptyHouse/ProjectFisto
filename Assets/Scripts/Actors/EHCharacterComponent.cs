using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHCharacterComponent : EHActorComponent
{
    public EHCharacter AssociatedCharacter { get; private set; }
    protected override void InitializeOwningActor()
    {
        base.InitializeOwningActor();
        AssociatedCharacter = (EHCharacter) AssociatedActor;
    }
}
