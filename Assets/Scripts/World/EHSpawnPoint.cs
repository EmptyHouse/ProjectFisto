using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHSpawnPoint : MonoBehaviour
{
    public bool IsFacingLeft;

    
    #region monobehaviour methods
    private void OnDrawGizmos()
    {
        EHDebug.DebugDrawFlag(transform.position, IsFacingLeft);
    }
    #endregion monobehaivour methods
}
