using EmptyHouseGames.Library;
using UnityEngine;
using UnityEditor;

public static class EHDebug
{
    public static void DebugDrawBox2D(FBox2D BoxParams)
    {
        DebugDrawBox2D(BoxParams, Color.red);
    }

    public static void DebugDrawBox2D(FBox2D BoxParams, Color BoxColor, bool Fill = false)
    {
#if UNITY_EDITOR
        Rect UnityRect = new Rect(BoxParams.Origin, BoxParams.Size);
        Color FillColor = BoxColor;
        if (Fill) FillColor.a = .25f;
        else FillColor.a = 0;
        Handles.DrawSolidRectangleWithOutline(UnityRect, FillColor, BoxColor);
#endif
    }

    public static void DebugDrawRightTriangle(FBox2D BoxParams, Color TriangleColor, bool Fill = false)
    {
        
    }

    public static void DebugDrawFlag(Vector2 OriginPosition, bool IsFacingLeft = false)
    {
#if UNITY_EDITOR
        Vector3 VecOriginPoint = OriginPosition;
        UnityEditor.Handles.color = Color.cyan;
        float SpawnPointHeight = .5f;
        UnityEditor.Handles.DrawSolidDisc(OriginPosition, Vector3.forward, .10f);
        Vector3 TopPoint = VecOriginPoint + Vector3.up * SpawnPointHeight;
        Vector3 MidPoint = VecOriginPoint + Vector3.up * SpawnPointHeight / 2;
        Vector3 RightPoint = (TopPoint + MidPoint) / 2 + ((IsFacingLeft ? Vector3.left : Vector3.right) * SpawnPointHeight / 2);
        UnityEditor.Handles.DrawLine(VecOriginPoint, TopPoint);
        UnityEditor.Handles.DrawPolyLine(new Vector3[] { TopPoint, MidPoint, RightPoint, TopPoint });
#endif
    }

    public static void DrawPoint(Vector2 Point, bool FillCircle = false, float Radius = 1)
    {
        DrawPoint(Point, Color.red, FillCircle, Radius);
    }

    public static void DrawPoint(Vector2 Point, Color DebugColor, bool FillCircle = false, float Radius = 1)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = DebugColor;
        Vector3 VecOriginPoint = Point;
        if (FillCircle)
            UnityEditor.Handles.DrawSolidDisc(Point, Vector3.forward, Radius);
        else
            UnityEditor.Handles.DrawSolidDisc(Point, Vector3.forward, Radius);
#endif
    }

    public static void DrawLine(Vector2 Origin, Vector2 EndPoint)
    {
        DrawLine(Origin, EndPoint, Color.red);
    }

    public static void DrawLine(Vector2 Origin, Vector2 EndPoint, Color DebugColor)
    {
        #if UNITY_EDITOR
        UnityEditor.Handles.color = DebugColor;
        UnityEditor.Handles.DrawLine(Origin, EndPoint);
        #endif
    }

    public static void DebugPrintByteArray(byte[] Data, bool DisplayHex = false)
    {
        string ByteDataString = "[";
        if (Data == null)
        {
            Debug.Log("NULL BYTE ARRAY");
        }
        for (int i = 0; i < Data.Length - 1; ++i)
        {
            ByteDataString += Data[i].ToString() + ", ";
        }
        if (Data.Length > 0)
        {
            ByteDataString += Data[Data.Length - 1].ToString();
        }
        ByteDataString += "]";
        Debug.Log(ByteDataString);
    }
}