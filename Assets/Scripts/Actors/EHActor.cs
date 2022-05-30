using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHActor : MonoBehaviour
{
    private Vector2 Position;
    private Vector2 Scale;
    private float ZRotation;

    protected Animator Anim;
    
    #region monobehaviour methods

    protected void Awake()
    {
        Anim = GetComponent<Animator>();
    }

    #endregion monobehaviour methods
    
    #region getter/setter methods

    public Vector2 GetPosition() => Position;
    public Vector2 GetScale() => Scale;
    public float GetRotation() => ZRotation;

    public void SetPosition(Vector2 Position)
    {
        this.Position = Position;
        transform.position = this.Position;
    }

    public void SetRotation(float Rotation)
    {
        this.ZRotation = Rotation;
        transform.rotation = Quaternion.Euler(0, 0, Rotation);
    }
    
    public void SetScale(Vector2 Scale)
    {
        this.Scale = Scale;
        this.transform.localScale = new Vector3(Scale.x, Scale.y, 1);
    }
    #endregion getter/setter methods
}
