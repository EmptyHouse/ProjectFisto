using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHPlayerInventory : EHActorComponent
{
    // NOTE: Remove Serialize field later
    [SerializeField]
    public List<EHInventoryItem> ActiveItems { get; private set; }
    private int CurrentActiveIndex = 0;

    protected void Awake()
    {
        ActiveItems = new List<EHInventoryItem>();
    }

    public EHInventoryItem GetCurrentActiveItem()
    {
        if (CurrentActiveIndex < 0 || CurrentActiveIndex >= ActiveItems.Count)
        {
            return null;
        }
        return ActiveItems[CurrentActiveIndex];
    }

    public void SetNextInventoryItem()
    {
        CurrentActiveIndex++;
        CurrentActiveIndex %= ActiveItems.Count;
    }

    public void SetPreviousInventoryItem()
    {
        CurrentActiveIndex--;
        if (CurrentActiveIndex < 0) CurrentActiveIndex += ActiveItems.Count;
        CurrentActiveIndex %= ActiveItems.Count;
    }

    public void AddActiveInventoryItem(EHInventoryItem ItemToAdd)
    {
        ActiveItems.Add(ItemToAdd);
    }

    public bool RemoveActiveInventoryItem(EHInventoryItem ItemToRemove)
    {
        if (!ActiveItems.Contains(ItemToRemove))
        {
            return false;
        }
        ActiveItems.Remove(ItemToRemove);
        return true;
    }
}
