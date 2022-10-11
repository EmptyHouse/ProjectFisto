using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHRoomDoor : EHActor
{
    [SerializeField]
    private Transform SpawnPosition;
    [SerializeField]
    private SceneField RoomToLoad;
    [SerializeField] 
    private int RoomDoorId;
    

    protected override void Awake()
    {
        base.Awake();
        ColliderComponent.OnOverlapBegin += OnPlayerOverlapBegin;
    }

    private void Start()
    {
        EHGameInstance.Instance.World.AddDoorToWorld(this);
    }

    private void OnValidate()
    {
        if (RoomDoorId <= 0)
        {
            RoomDoorId = GetRandomAvailableId();
        }
    }

    public void OnPlayerOverlapBegin(EHBoxCollider2D OtherCollider)
    {
        EHPlayerCharacter PlayerCharacter = OtherCollider.GetComponent<EHPlayerCharacter>();
        if (PlayerCharacter == null)
        {
            return;
        }

        EHGameInstance.Instance.LoadBackgroundScene(RoomToLoad, RoomDoorId, true);
    }

    public void LoadNewRoom()
    {
        
    }

    public Vector2 GetSpawnPosition() => SpawnPosition.position;

    public int GetRoomDoorId() => RoomDoorId;

    
    public static int GetRandomAvailableId()
    {
        int RandNum = UnityEngine.Random.Range(1, 100);
        EHRoomDoor[] RoomDoors = GameObject.FindObjectsOfType<EHRoomDoor>();
        foreach (EHRoomDoor RoomDoor in RoomDoors)
        {
            if (RandNum == RoomDoor.RoomDoorId)
            {
                return GetRandomAvailableId();
            }
        }
        return RandNum;
    }
}
