using UnityEngine;
using System.Collections.Generic;

namespace Helper
{
    public class ShadowHelper
    {
        public static void DrawVisibility(Vector2[] encounters, Vector2 lightposition, Color color = default(Color))
        {
            color = DebugDrawHelper.ValidateColor(color);

            for (int i = 1; i < encounters.Length; i++)
            {
                DebugDrawHelper.DrawLine(encounters[i - 1], encounters[i], color);
            }
        }
    } 
}
