using UnityEngine;

public class EHActor : MonoBehaviour
{
    public EHAnimatorComponent Anim { get; private set; }
    public EHBoxCollider2D ColliderComponent { get; private set; }
    public EHActor Owner { get; private set; }
    
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
        transform.Translate(OffsetPosition);
    }

    public void TranslateForward(float XOffset)
    {
        transform.Translate(Mathf.Sign(transform.localScale.x) * XOffset, 0, 0);
    }

    public void TranslateVertical(float YOffset)
    {
        transform.Translate(0, YOffset, 0);
    }

    public void SetActorActive(bool IsActive)
    {
        gameObject.SetActive(IsActive);
    }
    
    public EHGameInstance GetGameInstance() => EHGameInstance.Instance;

    public void SetOwner(EHActor Owner)
    {
        this.Owner = Owner;
    }
    
    public T GetGameMode<T>() where T : EHGameMode => (T)EHGameInstance.Instance.GameMode;

    public T GetGameState<T>() where T : EHGameState => (T) EHGameInstance.Instance.GameState;

    // We may want to make it so that the actor is given an owner when spawned from an actor
    public T SpawnActor<T>(T ActorToSpawn, Vector2 Position, float Rotation = 0) where T : EHActor
    {
        T NewActor = Instantiate(ActorToSpawn, Position, Quaternion.Euler(0, 0, Rotation));
        NewActor.SetOwner(this);
        return NewActor;
    }
    #endregion getter/setter methods
}
