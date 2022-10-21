using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHGameMode : MonoBehaviour
{
    public EHPhysics2DManager PhysicsManager { get; private set; }

    private float FreezeSecondsRemaining;
    
    #region monobehaviour methods

    protected virtual void Awake()
    {
        PhysicsManager = new EHPhysics2DManager();
    }

    public virtual void Update()
    {
        PhysicsManager.UpdatePhysicsLoop(EHTime.DeltaTime);
    }

    public void FreezeTime(float SecondsToFreeze)
    {
        StartCoroutine(FreezeTimeCoroutine(SecondsToFreeze));
    }
    #endregion

    private IEnumerator FreezeTimeCoroutine(float SecondsToFreeze)
    {
        if (FreezeSecondsRemaining > 0)
        {
            FreezeSecondsRemaining = Mathf.Max(SecondsToFreeze, FreezeSecondsRemaining);
            yield break;
        }
        FreezeSecondsRemaining = SecondsToFreeze;
        EHTime.TimeScale = 0;
        while (FreezeSecondsRemaining > 0)
        {
            FreezeSecondsRemaining -= Time.unscaledDeltaTime;
            yield return null;
        }
        EHTime.TimeScale = 1;
    }
}
