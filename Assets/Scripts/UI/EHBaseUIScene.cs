using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EHBaseUIScene : MonoBehaviour
{
    public bool IsShown { get; private set; }

    public virtual void OnShow()
    {
        IsShown = true;
    }

    public virtual void OnHide()
    {
        IsShown = false;
    }

    public void PushMe()
    {
        
    }

    public void PopMe()
    {
        EHBaseGameHUD GameHUD = GetGameHUD<EHBaseGameHUD>();
        if (GameHUD)
        {
            GameHUD.PopScene(this);
        }
    }

    protected T GetGameHUD<T>() where T : EHBaseGameHUD
    {
        if (EHGameInstance.Instance)
        {
            return (T)EHGameInstance.Instance.GameHUD;
        }

        return null;
    }
    
    #region selectable UI

    private List<EHSelectableUI> AllSelectableUI;
    private void InitializeSelectableUI()
    {
        EHSelectableUI[] AllSelectableUIComponents = GetComponentsInChildren<EHSelectableUI>();
        AllSelectableUI = AllSelectableUIComponents.ToList();
    }
    #endregion selectable UI
}
