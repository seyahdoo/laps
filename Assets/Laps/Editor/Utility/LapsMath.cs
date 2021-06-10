using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LapsEditor {
    public static class LapsMath {
        public static float DistanceFromPointToRect(Rect rect, Vector2 p) {
            //source MultiRRomero at https://stackoverflow.com/questions/5254838/calculating-distance-between-a-point-and-a-rectangular-box-nearest-point
            var dx = Mathf.Max(rect.min.x - p.x, 0, p.x - rect.max.x);
            var dy = Mathf.Max(rect.min.y - p.y, 0, p.y - rect.max.y);
            var distance = Mathf.Sqrt(dx * dx + dy * dy);
            return distance;
        }
    }
}
