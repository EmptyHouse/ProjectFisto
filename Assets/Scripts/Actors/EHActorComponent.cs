using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHActorComponent : MonoBehaviour
{
    public EHActor AssociatedActor { get; protected set; }

    #region monobehaviuor methods

    protected virtual void Awake()
    {
        InitializeOwningActor();
    }
    #endregion monobehaviour methods

    protected virtual void InitializeOwningActor()
    {
        AssociatedActor = GetComponent<EHActor>();
    }
    
    #region getter/setter functions

    public Vector2 GetActorPosition() => AssociatedActor.GetPosition();
    public Vector2 GetActorScale() => AssociatedActor.GetScale();
    public float GetActorRotation() => AssociatedActor.GetRotation();
    public EHActor GetOwningActor() => AssociatedActor.Owner;
    public void TranslateActorPosition(Vector2 OffsetPosition) => AssociatedActor.TranslatePosition(OffsetPosition);

    public void SetActorPosition(Vector2 Position) => AssociatedActor.SetPosition(Position);
    public void SetActorScale(Vector2 Scale) => AssociatedActor.SetScale(Scale);
    public void SetActorRotation(float ZRotation) => AssociatedActor.SetRotation(ZRotation);

    public EHGameInstance GetGameInstance() => AssociatedActor.GetGameInstance();

    public EHGameMode GetGameMode<T>() where T : EHGameMode => AssociatedActor.GetGameMode<T>();

    #endregion getter/setter functions
}
