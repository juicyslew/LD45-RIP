using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class Vector2Extension
    {
        public static Vector2 Slide(this Vector2 orig, Vector2 norm)
        {
            Vector2 perp = Vector2.Perpendicular(norm);
            perp = new Vector2(Mathf.Abs(perp.x), Mathf.Abs(perp.y));
            Vector2 vel = Vector2.Dot(orig, perp)/(perp.sqrMagnitude) * perp; // projection
            return vel;
        }
    }
}
