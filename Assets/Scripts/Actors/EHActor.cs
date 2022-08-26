using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EHAnimatorComponent))]
public class EHActor : MonoBehaviour
{
    public EHAnimatorComponent Anim { get; private set; }
    public EHBoxCollider2D ColliderComponent { get; private set; }
    
    #region monobehaviour methods

    protected virtual void Awake()
    {
        Anim = GetComponent<EHAnimatorComponent>();
        ColliderComponent = GetComponent<EHBoxCollider2D>();

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
    
    // Call this function so that we do not sweep collisions  when moving the character
    // NOTE: Keep in mind that this may cause the character to fall through collisions if improperly placed
    public void SetPositionNoSweep(Vector2 Position)
    {
        transform.position = Position;
        ColliderComponent.UpdateMoveableBoxCollider();
    }

    public void SetRotation(float Rotation)
    {
        transform.rotation = Quaternion.Euler(0, 0, Rotation);
    }
    
    public void SetScale(Vector2 Scale)
    {
        this.transform.localScale = new Vector3(Scale.x, Scale.y, 1);
    }

    public void TranslatePosition(Vector2 OffsetPosition)
    {
        this.transform.Translate(OffsetPosition);
    }
    
    public EHGameInstance GetGameInstance() => EHGameInstance.Instance;
    public T GetGameMode<T>() where T : EHGameMode => (T)EHGameInstance.Instance.GameMode;

    public T GetGameState<T>() where T : EHGameState => (T) EHGameInstance.Instance.GameState;

    #endregion getter/setter methods
}
