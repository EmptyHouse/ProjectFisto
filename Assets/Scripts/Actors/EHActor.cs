using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHActor : MonoBehaviour
{
    public Animator Anim { get; private set; }
    
    #region monobehaviour methods

    protected virtual void Awake()
    {
        Anim = GetComponent<Animator>();
    }

    #endregion monobehaviour methods
    
    #region getter/setter methods

    public Vector2 GetPosition() => transform.position;
    public Vector2 GetScale() => transform.localScale;
    public float GetRotation() => transform.eulerAngles.z;

    public void SetPosition(Vector2 Position)
    {
        transform.position = Position;
    }

    public void SetRotation(float Rotation)
    {
        transform.rotation = Quaternion.Euler(0, 0, Rotation);
    }
    
    public void SetScale(Vector2 Scale)
    {
        this.transform.localScale = new Vector3(Scale.x, Scale.y, 1);
    }
    
    public EHGameInstance GetGameInstance() => EHGameInstance.Instance;
    public T GetGameMode<T>() where T : EHGameMode => (T)EHGameInstance.Instance.GameMode;

    public T GetGameState<T>() where T : EHGameState => (T) EHGameInstance.Instance.GameState;

    #endregion getter/setter methods
}
