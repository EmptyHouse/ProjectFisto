using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EHUIRoutesTableRow : EHTableRow
{
    [Tooltip("This will be the first scene that is loaded. If not marked, we will use the first index as the first scene")]
    public bool IsInitialScene;
    [Tooltip("Scene prefab to load")]
    public EHBaseUIScene UIScene;
    [Tooltip("Is this a popup? Meaning that we will not hide the scene beneath this one")]
    public bool IsPopup;
    [Tooltip("The priority draw order for this scene")]
    public int Priority;
}

[CreateAssetMenu(fileName = "RoutesTable", menuName = "ScriptableObjects/RoutesTable", order = 1)]
public class EHUIRoutesTable : EHDataTable<EHUIRoutesTableRow>
{
    
}
