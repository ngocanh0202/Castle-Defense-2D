using System;
using UnityEngine;

namespace Common2D
{
    public static class HelperVector2
    {
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float radians = degrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);
            return new Vector2(
                v.x * cos - v.y * sin,
                v.x * sin + v.y * cos
            );
        }
        public static Vector2 ChangeToVector2(this Vector3 v, float z = 0f)
        {
            return new Vector2(v.x, v.y);
        }
        
    }
}
