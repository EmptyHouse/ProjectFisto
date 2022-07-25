using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHCameraFollow : MonoBehaviour
{
    private Camera CameraComponent;
    private EHActor FollowTarget;
    private Vector3 CameraOffset;
    [SerializeField, Tooltip("The rate at which our camera will lerp toward the player")] 
    private float CameraSpeed = 25;

    #region mononbehaviour methods

    protected void Awake()
    {
        CameraComponent = GetComponent<Camera>();
        FollowTarget = transform.parent.GetComponent<EHActor>();
        
        if (FollowTarget == null)
        {
            Debug.LogWarning("Follow Target was not set...");
            return;
        }
        
        Vector3 TargetPosition = FollowTarget.GetPosition();
        CameraOffset = transform.position - TargetPosition;
        //Detach from our parent object
        transform.SetParent(null);
    }

    protected void LateUpdate()
    {
        UpdateCameraPosition();
    }

    #endregion monobehaviour methods

    private void UpdateCameraPosition()
    {
        Vector3 TargetPosition = FollowTarget.GetPosition();
        Vector3 GoalPosition = CameraOffset + TargetPosition;
        transform.position = Vector3.Lerp(transform.position, GoalPosition, EHTime.DeltaTime * CameraSpeed);
    }
}
