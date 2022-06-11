using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHActorComponent : MonoBehaviour
{
    public EHActor OwningActor { get; protected set; }

    #region monobehaviuor methods

    protected virtual void Awake()
    {
        InitializeOwningActor();
    }
    #endregion monobehaviour methods

    protected virtual void InitializeOwningActor()
    {
        OwningActor = GetComponent<EHActor>();
    }
    
    #region getter/setter functions

    public Vector2 GetActorPosition() => OwningActor.GetPosition();
    public Vector2 GetActorScale() => OwningActor.GetScale();
    public float GetActorRotation() => OwningActor.GetRotation();

    public void SetActorPosition(Vector2 Position) => OwningActor.SetPosition(Position);
    public void SetActorScale(Vector2 Scale) => OwningActor.SetScale(Scale);
    public void SetActorRotation(float ZRotation) => OwningActor.SetRotation(ZRotation);

    public EHGameInstance GetGameInstance() => OwningActor.GetGameInstance();

    public EHGameMode GetGameMode<T>() where T : EHGameMode => OwningActor.GetGameMode<T>();

    #endregion getter/setter functions
}
