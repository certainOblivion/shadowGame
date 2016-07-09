using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Helper
{
    public class VectorHelper
    {
        /// <summary>
        /// Computes the intersection point of the line p1-p2 with p3-p4
        /// </summary>        
        public static Vector2 LineLineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            var s = ((p4.x - p3.x) * (p1.y - p3.y) - (p4.y - p3.y) * (p1.x - p3.x))
                    / ((p4.y - p3.y) * (p2.x - p1.x) - (p4.x - p3.x) * (p2.y - p1.y));
            return new Vector2(p1.x + s * (p2.x - p1.x), p1.y + s * (p2.y - p1.y));
        }

        /// <summary>
        /// Returns if the point is 'left' of the line p1-p2
        /// </summary>        
        public static bool LeftOf(Vector2 p1, Vector2 p2, Vector2 point)
        {
            float cross = (p2.x - p1.x) * (point.y - p1.y)
                        - (p2.y - p1.y) * (point.x - p1.x);

            return cross < 0;
        }

        /// <summary>
        /// Returns a slightly shortened version of the vector:
        /// p * (1 - f) + q * f
        /// </summary>        
        public static Vector2 Interpolate(Vector2 p, Vector2 q, float f)
        {
            return new Vector2(p.x * (1.0f - f) + q.x * f, p.y * (1.0f - f) + q.y * f);
        }

    } 

    public class MathHelper
    {
        public static float PiOver4 = 0.78539816339f;
        public static float TwoPi = 6.28318530718f;
        public static float Pi = 3.14159265359f;
        public static float Sqrt2 = 1.41421356237f;

        public static Vector2 RotatePoint(Vector2 point, Vector2 rotateAround, float angle)
        {
            float tempX = point.x - rotateAround.x;
            float tempY = point.y - rotateAround.y;

            // now apply rotation
            float rotatedX = tempX * Mathf.Cos(angle) - tempY * Mathf.Sin(angle);
            float rotatedY = tempX * Mathf.Sin(angle) + tempY * Mathf.Cos(angle);

            // translate back
            Vector2 rotatedPoint = new Vector2(rotatedX + rotateAround.x, rotatedY + rotateAround.y);

            return rotatedPoint;
        }

        public static Vector2[] GetRectangleCorners(Vector2 center, float width, float height, float rotationInRadians)
        {
            Vector2 topLeft = new Vector2(center.x - (width / 2), center.y + (height / 2));
            Vector2 topRight = new Vector2(center.x + (width / 2), center.y + (height / 2));
            Vector2 bottomLeft = new Vector2(center.x - (width / 2), center.y - (height / 2));
            Vector2 bottomRight = new Vector2(center.x + (width / 2), center.y - (height / 2));

            Vector2 rotatedTopLeft = RotatePoint(topLeft, center, rotationInRadians);
            Vector2 rotatedTopRight = RotatePoint(topRight, center, rotationInRadians);
            Vector2 rotatedBottomLeft = RotatePoint(bottomLeft, center, rotationInRadians);
            Vector2 rotatedBottomRight = RotatePoint(bottomRight, center, rotationInRadians);

            Vector2[] cornerList = { rotatedTopLeft, rotatedTopRight, rotatedBottomRight, rotatedBottomLeft };

            return cornerList;
        }
    }
}
