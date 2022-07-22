using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using EmptyHouseGames.Library;

public class EHAngledCollider2D : EHBoxCollider2D
{
    private enum EAngledDirection
    {
        Rotate0,
        Rotate90,
        Rotate180,
        Rotate270,
    }

    [SerializeField] 
    private EAngledDirection Direction = EAngledDirection.Rotate0;
    
    #region monobehaviour methods

    protected void OnValidate()
    {
        ColliderType = EColliderType.Static;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
    #endregion monobehaviour methods
    
    #region override methods

    public override bool PushOutCollider(EHBoxCollider2D OtherCollider, out Vector2 PushDirection)
    {
        if (base.PushOutCollider(OtherCollider, out PushDirection))
        {
            FBox2D OtherPreviousBox = OtherCollider.GetPreviousBoxBounds();
            FBox2D OtherCurrentBox = OtherCollider.GetCurrentBoxBounds();
            Vector2 OtherColliderCorner;
            Vector2 PreviousColliderCorner;
            switch (Direction)
            {
                case EAngledDirection.Rotate0:
                    if (OtherPreviousBox.MaxBounds.y < CurrentBox.MinBounds.y ||
                        OtherPreviousBox.MinBounds.x > CurrentBox.MaxBounds.x) return true;
                    
                    break;
                case EAngledDirection.Rotate90:
                    if (OtherPreviousBox.MaxBounds.y < CurrentBox.MinBounds.y ||
                        OtherPreviousBox.MaxBounds.x < CurrentBox.MinBounds.x) return true;
                    break;
                case EAngledDirection.Rotate180:
                    if (OtherPreviousBox.MinBounds.y > CurrentBox.MaxBounds.y ||
                        OtherPreviousBox.MaxBounds.x < CurrentBox.MinBounds.x) return true;
                    break;
                case EAngledDirection.Rotate270:
                    if (OtherPreviousBox.MinBounds.y > CurrentBox.MaxBounds.y ||
                        OtherPreviousBox.MinBounds.x > CurrentBox.MaxBounds.x) return true;
                    break;
            }
        }

        PushDirection = Vector2.zero;
        return false;
    }
    #endregion override methods
}
