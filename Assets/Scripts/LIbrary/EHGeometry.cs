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
                minBounds = value;
                maxBounds = value + size;
            }
        }

        public Vector2 Size
        {
            get => size;
            set
            {
                size = value;
                maxBounds = origin + value;
            }
        }

        public Vector2 MinBounds => minBounds;
        public Vector2 MaxBounds => maxBounds;
        
        private Vector2 origin;
        private Vector2 size;

        private Vector2 minBounds;
        private Vector2 maxBounds;

        public FBox2D(Vector2 Origin, Vector2 Size)
        {
            origin = Origin;
            size = Size;
            minBounds = Origin;
            maxBounds = Origin + Size;
        }

        public bool IsOverlappingBox2D(FBox2D OtherBox)
        {
            
            if (minBounds.x >= OtherBox.maxBounds.x || OtherBox.minBounds.x >= maxBounds.x) return false;
            if (minBounds.y >= OtherBox.maxBounds.y || OtherBox.minBounds.y >= maxBounds.y) return false;
            return true;
        }

        public float GetShortestDistance(FBox2D OtherBox)
        {
            bool Left = OtherBox.maxBounds.x < minBounds.x;
            bool Right = maxBounds.x < OtherBox.minBounds.x;
            bool Bottom = OtherBox.maxBounds.y < minBounds.y;
            bool Top = maxBounds.y < OtherBox.minBounds.y;
            
            if (Top && Left)
            {
                return Vector2.Distance(new Vector2(minBounds.x, maxBounds.y), new Vector2(OtherBox.maxBounds.x, OtherBox.minBounds.y));
            }
            else if (Left && Bottom)
            {
                return Vector2.Distance(minBounds, OtherBox.maxBounds);
            }
            else if (Bottom && Right)
            {
                return Vector2.Distance(new Vector2(maxBounds.x, minBounds.y), new Vector2(OtherBox.minBounds.x, OtherBox.maxBounds.y));
            }
            else if (Right && Top)
            {
                return Vector2.Distance(maxBounds, OtherBox.minBounds);
            }
            else if (Left)
            {
                return OtherBox.maxBounds.x - minBounds.x;
            }
            else if (Right)
            {
                return maxBounds.x - OtherBox.minBounds.x;
            }
            else if (Bottom)
            {
                return minBounds.y - OtherBox.maxBounds.y;
            }
            else if (Top)
            {
                return OtherBox.minBounds.y - maxBounds.y;
            }
            return 0;
        }
        
        // NOTE: You can remove the min and max bounds print... its just redundant info
        public override string ToString()
        {
            return
                string.Format($"Rect: {origin}\nSize: {size}\nMinBounds: {minBounds}\nMaxBounds: {maxBounds}");
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

