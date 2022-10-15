using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class EHWorld : MonoBehaviour
{
    private Dictionary<int, EHRoomDoor> DoorsInWorld = new Dictionary<int, EHRoomDoor>();

    private void Awake()
    {
        EHGameInstance.Instance.SetGameWorld(this);
    }

    public void AddDoorToWorld(EHRoomDoor RoomDoor)
    {
        DoorsInWorld.Add(RoomDoor.GetRoomDoorId(), RoomDoor);
    }

    public EHRoomDoor GetRoomDoorById(int RoomDoorId)
    {
        if (DoorsInWorld.ContainsKey(RoomDoorId))
        {
            return DoorsInWorld[RoomDoorId];
        }
        
        // We may just want to return a random door from the list, but still give them an error that the doors was not found
        Debug.LogWarning("RoomDoor Id: " + RoomDoorId + " was not found");
        return null;
    }
}
