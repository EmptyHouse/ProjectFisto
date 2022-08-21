using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class EHBaseGameHUD : MonoBehaviour
{
    public EHUIRoutesTable RoutesTable;
    // Scene that is currently interactable
    public EHBaseUIScene ActiveScene;
    // public StackList<EHBaseUIScene> SceneStack { get; private set; } = new StackList<EHBaseUIScene>();


    #region monobehaviour methods
    protected void Awake()
    {
        
    }
    #endregion monobehaviour methods

    public void PushScene(string SceneId)
    {
        
    }

    public void PopScene()
    {
        // if (SceneStack.Any())
        // {
        // }
    }

    public void PopScene(EHBaseUIScene Scene)
    {
        
    }
}
