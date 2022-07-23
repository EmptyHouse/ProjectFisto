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

    private float Slope;
    private float YInitial;
    
    #region monobehaviour methods

    protected void OnValidate()
    {
        ColliderType = EColliderType.Static;
    }

    protected override void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            if (!OwningActor) OwningActor = GetComponent<EHActor>();
            UpdateCurrentBoxGeometry();
        }

        UnityEditor.Handles.color = Color.green;
        Vector3[] Array;
        switch (Direction)
        {
            case EAngledDirection.Rotate0:
                Array = new Vector3[]
                {
                    CurrentBox.MaxBounds,
                    CurrentBox.MinBounds,
                    new Vector2(CurrentBox.MaxBounds.x, CurrentBox.MinBounds.y),
                    CurrentBox.MaxBounds,
                };
                break;
            case EAngledDirection.Rotate90:
                Array = new Vector3[]
                {
                    CurrentBox.MinBounds,
                    new Vector2(CurrentBox.MaxBounds.x, CurrentBox.MinBounds.y),
                    new Vector2(CurrentBox.MinBounds.x, CurrentBox.MaxBounds.y),
                    CurrentBox.MinBounds,
                };
                break;
            case EAngledDirection.Rotate180:
                Array = new Vector3[]
                {
                    CurrentBox.MaxBounds,
                    CurrentBox.MinBounds,
                    new Vector2(CurrentBox.MaxBounds.x, CurrentBox.MinBounds.y),
                    CurrentBox.MaxBounds,
                };
                break;
            case EAngledDirection.Rotate270:
                Array = new Vector3[]
                {
                    CurrentBox.MaxBounds,
                    new Vector2(CurrentBox.MaxBounds.x, CurrentBox.MinBounds.y),
                    new Vector2(CurrentBox.MinBounds.x, CurrentBox.MaxBounds.y),
                    CurrentBox.MaxBounds,
                };
                break;
            default: Array = new Vector3[] { };
                break;
        }
        UnityEditor.Handles.DrawPolyLine(Array);
    }
    #endregion monobehaviour methods
    
    #region override methods

    public override void UpdateCurrentBoxGeometry()
    {
        base.UpdateCurrentBoxGeometry();
        Vector2 MinVec = Vector2.zero;
        Vector2 MaxVec = Vector2.zero;
        switch (Direction)
        {
            case EAngledDirection.Rotate0:
                MinVec = CurrentBox.MinBounds;
                MaxVec = CurrentBox.MaxBounds;
                break;
            case EAngledDirection.Rotate90:
                MinVec = new Vector2(CurrentBox.MinBounds.x, CurrentBox.MaxBounds.y);
                MaxVec = new Vector2(CurrentBox.MaxBounds.x, CurrentBox.MinBounds.y);
                break;
            case EAngledDirection.Rotate180:
                MinVec = CurrentBox.MinBounds;
                MaxVec = CurrentBox.MaxBounds;
                break;
            case EAngledDirection.Rotate270:
                MinVec = new Vector2(CurrentBox.MinBounds.x, CurrentBox.MaxBounds.y);
                MaxVec = new Vector2(CurrentBox.MaxBounds.x, CurrentBox.MinBounds.y);
                break;
        }
        Vector2 SlopeVec = MaxVec - MinVec;
        YInitial = MinVec.y;
        Slope = SlopeVec.y / SlopeVec.x;
    }

    public override bool PushOutCollider(EHBoxCollider2D OtherCollider, out Vector2 PushDirection)
    {
        FBox2D OtherPreviousBox = OtherCollider.GetPreviousBoxBounds();
        FBox2D OtherCurrentBox = OtherCollider.GetCurrentBoxBounds();
        PushDirection = Vector2.zero;
        Vector2 OtherColliderCorner;
        float Y;
        switch (Direction)
        {
            case EAngledDirection.Rotate0:
                if (OtherPreviousBox.MaxBounds.y < CurrentBox.MinBounds.y ||
                    OtherPreviousBox.MinBounds.x > CurrentBox.MaxBounds.x) return base.PushOutCollider(OtherCollider, out PushDirection);
                OtherColliderCorner = new Vector2(OtherCurrentBox.MaxBounds.x, OtherCurrentBox.MinBounds.y);
                if (OtherColliderCorner.x > CurrentBox.MaxBounds.x) return base.PushOutCollider(OtherCollider, out PushDirection);
                Y = GetYValueBetweenPoint(OtherColliderCorner.x);
                if (OtherColliderCorner.y > Y)  return false;
                PushDirection = new Vector2(0, Y - OtherColliderCorner.y);
                return true;
            case EAngledDirection.Rotate90:
                if (OtherPreviousBox.MaxBounds.y < CurrentBox.MinBounds.y ||
                    OtherPreviousBox.MaxBounds.x < CurrentBox.MinBounds.x) return base.PushOutCollider(OtherCollider,out PushDirection);
                OtherColliderCorner = OtherCurrentBox.MinBounds;
                if (OtherColliderCorner.x < CurrentBox.MinBounds.x) return base.PushOutCollider(OtherCollider, out PushDirection);
                Y = GetYValueBetweenPoint(OtherColliderCorner.x);
                if (OtherColliderCorner.y > Y) return false;
                PushDirection = new Vector2(0, Y - OtherColliderCorner.y);
                return true;
            case EAngledDirection.Rotate180:
                if (OtherPreviousBox.MinBounds.y > CurrentBox.MaxBounds.y ||
                    OtherPreviousBox.MaxBounds.x < CurrentBox.MinBounds.x) return base.PushOutCollider(OtherCollider, out PushDirection);
                OtherColliderCorner = new Vector2(OtherCurrentBox.MinBounds.x, OtherCurrentBox.MaxBounds.y);
                if (OtherColliderCorner.x < CurrentBox.MinBounds.x) return base.PushOutCollider(OtherCollider, out PushDirection);
                Y = GetYValueBetweenPoint(OtherColliderCorner.x);
                if (OtherColliderCorner.y < Y) return false;
                PushDirection = new Vector2(0, Y - OtherColliderCorner.y);
                break;
            case EAngledDirection.Rotate270:
                if (OtherPreviousBox.MinBounds.y > CurrentBox.MaxBounds.y ||
                    OtherPreviousBox.MinBounds.x > CurrentBox.MaxBounds.x) return base.PushOutCollider(OtherCollider, out PushDirection);
                OtherColliderCorner = OtherCurrentBox.MaxBounds;
                if (OtherColliderCorner.x > CurrentBox.MaxBounds.x) return base.PushOutCollider(OtherCollider, out PushDirection);
                Y = GetYValueBetweenPoint(OtherColliderCorner.x);
                if (OtherColliderCorner.y < Y) return false;
                PushDirection = new Vector2(0, Y - OtherColliderCorner.y);
                break;
        }

        PushDirection = Vector2.zero;
        return false;
    }
    #endregion override methods

    private float GetYValueBetweenPoint(float X)
    {
        float XOffset = X - CurrentBox.MinBounds.x;
        return YInitial + (XOffset * Slope);
    }
}
