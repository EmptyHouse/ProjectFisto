using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace EmptyHouseGames.Library
{

    public struct FBox2D
    {
        public Vector2 Origin
        {
            get => origin;
            set
            {
                origin = value;
                MinBounds = value;
                MaxBounds = value + size;
            }
        }

        public Vector2 Size
        {
            get => size;
            set
            {
                size = value;
                MaxBounds = origin + value;
            }
        }
        
        private Vector2 origin;
        private Vector2 size;

        private Vector2 MinBounds;
        private Vector2 MaxBounds;

        FBox2D(Vector2 Origin, Vector2 Size)
        {
            origin = Origin;
            size = Size;
            MinBounds = Origin;
            MaxBounds = Origin + Size;
        }

        public bool IsOverlappingBox2D(FBox2D OtherBox)
        {
            
            if (MinBounds.x >= OtherBox.MaxBounds.x || OtherBox.MinBounds.x >= MaxBounds.x) return false;
            if (MinBounds.y >= OtherBox.MaxBounds.y || OtherBox.MinBounds.y >= MaxBounds.y) return false;
            return true;
        }

        public float GetShortestDistance(FBox2D OtherBox)
        {
            bool Left = OtherBox.MaxBounds.x < MinBounds.x;
            bool Right = MaxBounds.x < OtherBox.MinBounds.x;
            bool Bottom = OtherBox.MaxBounds.y < MinBounds.y;
            bool Top = MaxBounds.y < OtherBox.MinBounds.y;
            
            if (Top && Left)
            {
                return Vector2.Distance(new Vector2(MinBounds.x, MaxBounds.y), new Vector2(OtherBox.MaxBounds.x, OtherBox.MinBounds.y));
            }
            else if (Left && Bottom)
            {
                return Vector2.Distance(MinBounds, OtherBox.MaxBounds);
            }
            else if (Bottom && Right)
            {
                return Vector2.Distance(new Vector2(MaxBounds.x, MinBounds.y), new Vector2(OtherBox.MinBounds.x, OtherBox.MaxBounds.y));
            }
            else if (Right && Top)
            {
                return Vector2.Distance(MaxBounds, OtherBox.MinBounds);
            }
            else if (Left)
            {
                return OtherBox.MaxBounds.x - MinBounds.x;
            }
            else if (Right)
            {
                return MaxBounds.x - OtherBox.MinBounds.x;
            }
            else if (Bottom)
            {
                return MinBounds.y - OtherBox.MaxBounds.y;
            }
            else if (Top)
            {
                return OtherBox.MinBounds.y - MaxBounds.y;
            }
            return 0;
        }
        
        // NOTE: You can remove the min and max bounds print... its just redundant info
        public override string ToString()
        {
            return
                string.Format($"Rect: {origin}\nSize: {size}\nMinBounds: {MinBounds}\nMaxBounds: {MaxBounds}");
        }
        
        #region debug methods

        public static void DebugDrawRect(FBox2D Box)
        {
            DebugDrawRect(Box, Color.red);
        }

        public static void DebugDrawRect(FBox2D Box, Color DebugColor, bool IsFill = false)
        {
            #if UNITY_EDITOR
            Rect UnityRect = new Rect(Box.origin, Box.size);
            Color FillColor = DebugColor;

            if (IsFill) FillColor.a *= .25f;
            else FillColor.a = 0;
            UnityEditor.Handles.DrawSolidRectangleWithOutline(UnityRect, FillColor, DebugColor);
            #endif
        }
        #endregion debug methods
    }
}

