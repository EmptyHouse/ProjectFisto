using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHAnimatorComponent : EHActorComponent
{
    private const float TriggerBufferTime = 8f / 60f;
    private Animator Anim;
    private Dictionary<int, float> BufferTimeMap = new Dictionary<int, float>();
    #region monobehaviour methods

    protected override void Awake()
    {
        base.Awake();
        Anim = GetComponent<Animator>();
    }

    protected void Update()
    {
        if (BufferTimeMap.Count == 0) return;
        List<int> RemoveBuffers = new List<int>();
        
        foreach (var BufferValue in BufferTimeMap.Keys)
        {
            BufferTimeMap[BufferValue] -= Time.deltaTime;
            if (BufferTimeMap[BufferValue] <= 0)
            {
                RemoveBuffers.Add(BufferValue);
                ResetTrigger(BufferValue);
            }
        }

        foreach (int BufferToRemove in RemoveBuffers) BufferTimeMap.Remove(BufferToRemove);
    }
    #endregion monobehaviour methods
    
    #region set anim trigger

    public void SetFloat(int ParameterHash, float Value)
    {
        Anim.SetFloat(ParameterHash, Value);
    }

    public void SetInteger(int ParameterHash, int Value)
    {
        Anim.SetFloat(ParameterHash, Value);
    }

    public void SetBool(int ParameterHash, bool Value)
    {
        Anim.SetBool(ParameterHash, Value);
    }

    public void SetTrigger(int ParameterHash, float BufferTime = TriggerBufferTime)
    {
        Anim.SetTrigger(ParameterHash);
        // If buffer time is less than 0 we will skip adding it to our buffer
        if (BufferTime < 0) return;
        if (!BufferTimeMap.ContainsKey(ParameterHash))
        {
            BufferTimeMap.Add(ParameterHash, 0);
        }
        
        BufferTimeMap[ParameterHash] = BufferTime;
    }

    public void ResetTrigger(int ParameterHash)
    {
        Anim.ResetTrigger(ParameterHash);
        if (BufferTimeMap.ContainsKey(ParameterHash)) BufferTimeMap.Remove(ParameterHash);
    }

    public bool GetBool(int ParameterHash) => Anim.GetBool(ParameterHash);

    public float GetFloat(int ParameterHash) => Anim.GetFloat(ParameterHash);

    public int GetInteger(int ParameterHash) => Anim.GetInteger(ParameterHash);

    #endregion set anim trigger
}
