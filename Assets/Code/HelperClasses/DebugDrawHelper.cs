using UnityEngine;
using System.Collections;
using Grid;
using System.Collections.Generic;

#if UNITY_EDITOR
namespace Helper
{
    public class DebugDrawHelper
    {

        public static void DrawHex(Layout layout, Hex hex, Color color, bool filled = false)
        {
            List<Point> corners = Layout.PolygonCorners(layout, hex);
            List<Vector2> arrayOfCorners = new List<Vector2>(corners.Count);

            foreach (Point corner in corners)
            {
                arrayOfCorners.Add(GridHelper.PointToVector2(corner));
            }

            DebugDrawHelper.DrawHex(arrayOfCorners.ToArray(), color, filled);
        }

        public static Color ValidateColor(Color color)
        {
            return (color == default(Color)) ? Color.black : color;
        }
        public static void DrawHex(Vector2[] corners, Color color = default(Color), bool filled = false)
        {
            if (corners.Length != 6)
            {
                return;
            }

            color = ValidateColor(color);

            Vector3 startPoint;
            Vector3 endPoint;
            if (!filled)
            {
                startPoint = corners[0];
                for (int i = 1; i < corners.Length; i++)
                {
                    endPoint = corners[i];

                    DebugDrawHelper.DrawLine(startPoint, endPoint, color, depthTest: false);

                    startPoint = endPoint;
                }
            }
            else
            {
                for (int i = 0; i < corners.Length; i++)
                {
                    startPoint = corners[i];
                    for (int j = 0; j < corners.Length; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        endPoint = corners[j];

                        DebugDrawHelper.DrawLine(startPoint, endPoint, color, depthTest: false);
                    }
                }
            }
        }
        public static void DrawRectangle(Vector2 []corners, Color color = default(Color))
        {
            if (corners.Length != 4)
            {
                return;
            }

            color = ValidateColor(color);

            DrawLine(corners[0], corners[1], color);
            DrawLine(corners[1], corners[2], color);
            DrawLine(corners[2], corners[3], color);
            DrawLine(corners[3], corners[0], color);
        }

        public static void DrawRectangle(Vector2 center, float width, float height, float rotation = 0.0f, Color color = default(Color))
        { 
            DrawRectangle(MathHelper.GetRectangleCorners(center, width, height, 0), color);
        }

        public static void DrawTriangle(Vector2[] corners, Color color = default(Color))
        {
            if (corners.Length != 3)
            {
                return;
            }

            color = ValidateColor(color);

            DrawLine(corners[0], corners[1], color);
            DrawLine(corners[1], corners[2], color);
            DrawLine(corners[2], corners[0], color);
        }

        public static void DrawLine(Vector2 start, Vector2 end, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
        {
            color = ValidateColor(color);
            Debug.DrawLine(start, end, color, duration, depthTest);
        }

    }
}
#endif
