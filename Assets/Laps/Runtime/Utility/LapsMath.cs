using UnityEngine;

namespace LapsRuntime {
    public class LapsMath {
        public const float Epsilon = 0.0001f;
        public static float DistanceFromPointToRect(Rect rect, Vector2 p) {
            //source MultiRRomero at https://stackoverflow.com/questions/5254838/calculating-distance-between-a-point-and-a-rectangular-box-nearest-point
            var dx = Mathf.Max(rect.min.x - p.x, 0, p.x - rect.max.x);
            var dy = Mathf.Max(rect.min.y - p.y, 0, p.y - rect.max.y);
            var distance = Mathf.Sqrt(dx * dx + dy * dy);
            return distance;
        }
        public static bool ApproximatelyClose(float f1, float f2) {
            return Mathf.Abs(f1 - f2) < Epsilon;
        }
    }
}
